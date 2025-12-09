using ErpManagement.Domain.Models;
using Net.YSolution.Sac.Recruitment.Domain.Constants.Enums;

namespace Net.YSolution.Sac.Recruitment.Domain.Models.Shared;

[Table("Shar_Definition")]
public class SharDefinition : FullStaticDataEntity
{
    public required DefinitionEnum Key { get; set; }

}
