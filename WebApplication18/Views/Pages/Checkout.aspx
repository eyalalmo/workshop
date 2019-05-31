<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="WebApplication18.Views.Pages.Checkout" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>

 <div class="form-group">
  <label for="usr">Address:</label>
  <input type="text" class="form-control" id="address" name="address">
</div>
  <div class="form-group">
    <label for="creditcard">Credit Card:</label>
    <input type="text" class="form-control" id="creditcard" name="creditcard">
  </div>
  <div id="totalPrice" class ="form-group"></div>
  <button type="submit" name="btnPurchase" id="btnPurchase" class="btn btn-default">Purchase</button>

   <script type="text/javascript">

       $(document).ready(function () {

           var mainDiv = document.getElementById('totalPrice');
           var getUrl = window.location;
           var baseUrl = getUrl.protocol + "//" + getUrl.host
                  jQuery.ajax({
                      type: "GET",

                    url: baseUrl+"/api/user/basketTotalPrice",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                      success: function (response) {
                          if (response != "") {    
                             var total = response.split(",");

                            var str = "<label for=\"usr\">Total Price: $"+total[0]+"</label>";
                            mainDiv.innerHTML = str;
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
           
            $("#btnPurchase").click(function () {
                event.preventDefault();
                address = $("#address").val();
                creditcard = $("#creditcard").val();

                
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/user/Checkout?address=" + address + "&creditcard=" + creditcard,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "") {
                            window.location.href = baseUrl+"/";
                            alert("Purchase ended sucessfully\nThe delivery is on the way!")                     
                        }
                        else {
                            alert(response);
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

