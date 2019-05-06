<%@ Page Language="C#" Title="Admin Panel" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="AdminPan.aspx.cs" Inherits="WebApplication18.Pages.AdminPan" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
<div class="container">
  <h2>Contextual Classes</h2>
  <p>Contextual classes can be used to color table rows or table cells. The classes that can be used are: .active, .success, .info, .warning, and .danger.</p>
  <table id="usersTable" class="table">
    <thead>
      <tr>
        <th>Firstname</th>
        <th>Lastname</th>
        <th>Delete</th>
      </tr>
    </thead>
  </table>
</div>

   <script type="text/javascript">


       $(document).ready(function () {
           $("#userTable tbody").html("<tbody><tr>\n <td id=\"def1\">Default</td>\n <td>Defaultson</td>\n <td><button id=\"del1\" type=\"button\" class=\"btn btn-danger\">Danger</button></td> \n</tr> </tbody>");
            $("#del1").click(function () {
                event.preventDefault();
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                if (confirm("Are you sure you want to delete ____ ?")) {
                    
                    jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/AdminPan/removeUser?username=nfds",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response == "ok") {   
                            window.location.href = baseUrl+"/api/Default/";
                        }
                        else {
                            alert(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
                } else {
                    ////////
                }
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            });
        });

    </script>

</asp:Content>
