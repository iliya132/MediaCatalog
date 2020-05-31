using MediaCatalog.Model.BaseEntity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCatalog.Model.DTO
{
    [Table("MediaFile")]
    public class MediaFileDTO :NamedEntity
    {
        [Column("Timing")]
        public int TimingInFrames { get; set; }
        public string CompleteName { get; set; }
        public string Format { get; set; }
        public string FrameSize { get; set; }
        [Column("ParentProgram")]
        public int ParentTvProgramId { get; set; }

        [ForeignKey("ParentTvProgramId")]
        public virtual TV_ProgramDTO ParentProgram { get; set; }
    }
}
