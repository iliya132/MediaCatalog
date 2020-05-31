using MediaCatalog.Model.BaseEntity;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaCatalog.Model.DTO
{
    [Table("TVProgram")]
    public class TV_ProgramDTO :NamedEntity
    {
        public string Description { get; set; }
        public string Actors { get; set; }
        public int YearEstablished { get; set; }
        [Column("AvatarFilePath")]
        public string AvatarSourcePath { get; set; }
        private ObservableCollection<MediaFileDTO> _mediaFiles = new ObservableCollection<MediaFileDTO>();
        public ObservableCollection<MediaFileDTO> MediaFiles
        {
            get
            {
                return _mediaFiles;
            }
            set
            {
                _mediaFiles = value;
            }
        }
    }
}
