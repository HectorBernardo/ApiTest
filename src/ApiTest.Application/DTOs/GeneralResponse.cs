namespace ApiTest.Application.DTOs;

public record CommandResponse(bool Success, string Message, int? Id = null);