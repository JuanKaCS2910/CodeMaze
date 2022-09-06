namespace CompanyEmployees.Extensions
{
    public sealed class CollectionByIdsBadRequesttsException : BadRequestException
    {
        public CollectionByIdsBadRequesttsException() :base("Collection count mismatch comparing to ids.")
        {

        }
    }
}
