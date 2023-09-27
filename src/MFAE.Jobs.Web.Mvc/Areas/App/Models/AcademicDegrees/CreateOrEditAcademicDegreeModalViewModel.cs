using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.AcademicDegrees
{
    public class CreateOrEditAcademicDegreeModalViewModel
    {
        public CreateOrEditAcademicDegreeDto AcademicDegree { get; set; }

        public bool IsEditMode => AcademicDegree.Id.HasValue;
    }
}