using MinimalApi.Domain.Dto;
using MinimalApi.Domain.ModelViews;
using Administrator = MinimalApi.Domain.Dto.Administrator;

namespace MinimalApi.Validators;

public static class AdminRequestValidation
{
    public static ValidationErrors ValidateCreateAdminRequest(Administrator admin)
    {
        var errorMessages = new ValidationErrors
        {
            Messages = []
        };

        if (string.IsNullOrEmpty(admin.Email))
        {
            errorMessages.Messages.Add("Email can not be empty");
        }

        if (string.IsNullOrEmpty(admin.Password))
        {
            errorMessages.Messages.Add("Password can not be empty");
        }

        if (admin.Profile != 0)
        {
            errorMessages.Messages.Add("You are not an administrator.");
        }


        return errorMessages;

    }
}