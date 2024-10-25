using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
  public class RoomViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Style { get; set; }
    public int? Capacity { get; set; }
    public string Status { get; set; }
    public int? Floor { get; set; }
    public string Image { get; set; }
    public string Details { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedAt { get; set; }
  }

  public class RoomListViewModel
  {
    [DisplayName("ID")]
    public string IdFilter { get; set; }

    [Display(Name = "Name", ResourceType = typeof(Resources.Views.Screen))]
    public string NameFilter { get; set; }

    public IEnumerable<RoomViewModel> dataList { get; set; }
  }

  public class RoomPageViewModel
  {
    public RoomListViewModel RoomList { get; set; }
    public RoomViewModel NewRoom { get; set; }
  }
}
