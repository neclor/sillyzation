using System.Drawing;

namespace Shared.Dto;

public record PlayerDto(Guid Id, string Name, Color Color, bool IsReady);
