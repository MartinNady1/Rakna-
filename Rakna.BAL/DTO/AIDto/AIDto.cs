using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace Rakna.BAL.DTO.AIDto
{
    public class DetectionResultDto
    {
        public int Plate { get; set; }

        [JsonPropertyName("Object_Detected")]
        public double ObjectDetected { get; set; }

        [JsonPropertyName("Bounding Box (Object)")]
        public int[] BoundingBoxObject { get; set; }

        [JsonPropertyName("Confidence (Object)")]
        public double ConfidenceObject { get; set; }

        [JsonPropertyName("Characters result")]
        public List<CharacterResultDto> CharactersResult { get; set; }
    }

    public class CharacterResultDto
    {
        public string Character { get; set; }
        public double Confidence { get; set; }
    }
}

