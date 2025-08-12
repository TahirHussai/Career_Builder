namespace CareerBuilder.API.Models
{
    public class MLSettings
    {
        public string ModelPath { get; set; }
        public float MinimumConfidenceScore { get; set; }
        public int MaxTokenLength { get; set; }
        public string[] SkillsVocabulary { get; set; }
        public string[] DegreeTypes { get; set; }
        public string[] IndustryTypes { get; set; }
        public NLPSettings NLP { get; set; }
    }

    public class NLPSettings
    {
        public string LanguageModel { get; set; }
        public bool UseSpaCy { get; set; }
        public bool UseNLTK { get; set; }
        public float EntityRecognitionThreshold { get; set; }
        public string[] CustomEntities { get; set; }
    }
} 