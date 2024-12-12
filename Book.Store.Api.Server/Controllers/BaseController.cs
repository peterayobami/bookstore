using Microsoft.AspNetCore.Mvc;

namespace Book.Store.Api.Server
{
    // [ApiController]
    public class BaseController<TController> : ControllerBase
    {
        private readonly ILogger<TController> logger;

        public BaseController(ILogger<TController> logger)
        {
            this.logger = logger;
        }

        public async Task<ActionResult> HandleOperationAsync(Func<Task<OperationResult>> handleOperation)
        {
            try
            {
                var operation = await handleOperation();

                // If failed...
                if (!operation.Successful)
                {
                    return Problem(title: operation.ErrorTitle,
                        statusCode: operation.StatusCode, detail: operation.ErrorMessage);
                }

                return SuccessResponse(operation);
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex.Message);

                return Problem(title: "SYSTEM ERROR",
                    statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            }
        }

        private ActionResult SuccessResponse(OperationResult operation)
        {
            switch (operation.StatusCode)
            {
                case StatusCodes.Status200OK:
                    return Ok(operation.Result);
                case StatusCodes.Status201Created:
                    return Created("", operation.Result);
                case StatusCodes.Status204NoContent:
                    return NoContent();
                default:
                    return Ok(operation.Result);
            }
        }
    }
}