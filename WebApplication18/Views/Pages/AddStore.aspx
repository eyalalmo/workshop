<%@ Page Title="Add new Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.AddStore" %>



<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
    <div class="form-group">
  <label for="usr">Name Of The New Store:</label>
  <input type="text" class="form-control" id="name" name="name">
</div>
  <div class="form-group">
    <label for="pwd">Description:</label>
    <input type="text" class="form-control" id="des" name="des">
  </div>
  
  <button type="submit" name="btnAdd" id="btnAdd" class="btn btn-default">Submit</button>
 
   <script type="text/javascript">

        $(document).ready(function () {
            
            $("#btnAdd").click(function () {
                event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                name = $("#name").val();
                des = $("#des").val();
                 
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/addStore?name=" + name + "&description=" + des,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response != "fail") {
                            alert("store added success, id:" + response)                 
                           
                        }
                        else {
                            alert("store added failes")    
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            });
        });

    </script>

</asp:Content>

