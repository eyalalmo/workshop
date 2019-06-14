<%@ Page Title="Product Discount" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Discounts.aspx.cs" Inherits="WebApplication18.Views.Pages.Discounts" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <h4>Add Visible Discount:</h4>
  <label for="per">Percentage:</label>
  <input type="number" min="0" max="1" class="form-control" id="percentage" name="percentage">
  <label for="per">Duration:</label>
  <input type="text" class="form-control" id="duration" name="duration">
</div>
    <button type="submit" name="btnVisibleDiscount" id="btnVisibleDiscount" class="btn btn-default">Submit</button>

   <script type="text/javascript">

       $(document).ready(function () {
           var productID =<%=ViewData["productID"]%>;
            $("#btnVisibleDiscount").click(function () {
                event.preventDefault();
                percentage = $("#percentage").val();
                duration = $("#duration").val();    
                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/WebApplication18";
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/products/addVisibleDiscount?productID=" + productID + "&percentage=" + percentage+"&duration=" + duration,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "") {
                            location.reload();
                            alert("Visible Discount added Successfully")                     
                        }
                        else {
                            alert(response);
                            location.reload();
                        }
                    },
                    error: function (response) {
                        alert(response);
                        location.reload();
                    }
                });
            });
        
        });

    </script>

</asp:Content>

