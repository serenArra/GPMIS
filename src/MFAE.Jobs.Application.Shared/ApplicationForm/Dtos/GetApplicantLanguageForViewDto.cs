namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantLanguageForViewDto
    {
        public ApplicantLanguageDto ApplicantLanguage { get; set; }

        public string ApplicantFirstName { get; set; }

        public string LanguageName { get; set; }

        public string ConversationName { get; set; }
        
        public string ConversationRateName { get; set; }
        public string ConversationReadRate { get; set; }
        public string ConversationWriteRate { get; set; }
        public string ConversationspeakRate { get; set; }


    }
}