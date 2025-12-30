using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rakna.BAL.DTO.AIDto
{
    public class DetectionInputDto
    {
        public List<DetectionResultDto> Results { get; set; }
        public string Token { get; set; }
    }

}
