<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs"  Inherits="WebApplication18.Views.Pages.register" %>

<asp:Content ID="BodyContent1"  ContentPlaceHolderID="MainContent" runat="server">
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
    <div class="form-group">
    <label for="pwd">Reapet Password:</label>
    <input type="password" class="form-control" id="pass2" name="pass2">
  </div>
  
  <button type="submit" name="btnregister" id="btnregister" class="btn btn-primary">Submit</button>

  
     <script type="text/javascript">

        $(document).ready(function () {
            
            $("#btnregister").click(function () {
                event.preventDefault();
               var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18"
                username = $("#name").val();
                pass = $("#pass").val();
                pass2 = $("#pass2").val();
               
                
                if (pass !== pass2) {
                    window.alert("pass isnt the same")
                    return;
                }

                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/register?username=" + username + "&password=" + pass,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {          
                        if (response == "ok") {
                            alert("Registered Successfuly")
                            window.location.href = baseUrl+"/";
                        }
                        else {
                       alert("register failed "+response);  
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

