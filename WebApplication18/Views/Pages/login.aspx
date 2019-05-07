<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication18.Views.Pages.login" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <label for="usr">Name:</label>
  <input type="text" class="form-control" id="name" name="name">
</div>
  <div class="form-group">
    <label for="pwd">Password:</label>
    <input type="password" class="form-control" id="pass" name="pass">
  </div>
  <div class="checkbox">
    <label><input type="checkbox"> Remember me</label>
  </div>
  <button type="submit" name="btnLogin" id="btnLogin" class="btn btn-default">Submit</button>

   <script type="text/javascript">

        $(document).ready(function () {
            
            $("#btnLogin").click(function () {
                event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
                console.log(baseUrl);
                username = $("#name").val();
                pass = $("#pass").val();

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/login?username=" + username + "&password=" + pass,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {
                                                  
                            window.location.href = baseUrl+"/";
                        }
                        else {
                            console.log(response);   
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

