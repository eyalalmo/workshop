<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreDiscounts.aspx.cs" Inherits="WebApplication18.Views.Pages.StoreDiscounts" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
   <div class="container">
            <div id="Discounts" class ="col-md-12">  
            </div>
        <div id="addDiscount" class ="col-md-12">
            </div>   
        </div>
      <script type="text/javascript">

        $(document).ready(function () {
            
            var mainDiv = document.getElementById('Discounts');
             var addDiscountDiv = document.getElementById('addDiscount');
               
               var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host
                var storeID =<%=ViewData["storeID"]%>;
              
                console.log("before jquery");
                jQuery.ajax({
                    type: "GET",
                    url: baseUrl + "/api/store/StoreDiscounts?storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success:
                        function (response) {
                            console.log("response");
                            if (response != "") {
                                console.log(response);

                                var str = "<div class=\"card-body\"><div class=\"row\style=\"background-color:black\"> </div></div>";
                                var discounts = response.split(";");
                                for (i = 0; i < discounts.length - 1; i++) {
                                    var discountfields = discounts[i].split(",");
                                    var type = discountfields[0];
                                    var description = discountfields[1];
                                    var percentage = discountfields[2];
                                    var duration = discountfields[3];
                                    var discountID = discountfields[4];
                                    str +=
                                        "<hr><div class=\"row\">" +
                                        "<div class=\"row\"> <div class=\"col-12 text-sm-center col-sm-12 text-md-left col-md-6\"><h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></div><div class=\"col-8 col-sm-8  text-sm-center col-md-4 text-md-right row\"> <div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusQuantity(" + discountID + "," + percentage+");\" class=\"plus\"><input type=\"text\" value=\"" + percentage + " %\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusQuantity(" + discountID + "," + percentage+");\" class=\"minus\"></div><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"button\"  class=\"btn btn-secondary\" value=\"Complex\" onclick=\"deleteRow("+discountID+");\"></div> <div class=\"col-4 col-sm-4 col-md-4  text-right\"><button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow("+discountID+");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></div><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"checkbox\"></div></div></div></div>";
                                }
                                console.log(str);
                                mainDiv.innerHTML = str;
                                addDiscountDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Add Discount\" onclick=\" addDiscount("+storeID+")\"></div>";

                        }
                        else {
                                mainDiv.innerHTML = "<div align=\"center\"> <img src=\"../Images/nodiscount_2.png\"" + "height=\"400\" /></div>";
                                addDiscountDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" > <h5><strong>Store does not have Discounts</strong></h5> <input type=\"button\" class=\"btn btn-secondary\" value=\"Add Discount\" onclick=\" addDiscount("+storeID+")\"></div>";

                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          });







          function plusQuantity(id, percentage) {
              event.preventDefault();
              console.log(id);
              var plusPercentage = percentage + 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setDiscountPercentage?discountID=" + id+"&percentage="+plusPercentage,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }
          
          function minusQuantity(id, percentage) {
              event.preventDefault();
              console.log(id);
              var minusPercentage = percentage - 1;
              var getUrl = window.location;
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                    type: "GET",
                    url: baseUrl+"/api/store/setDiscountPercentage?discountID=" + id+"&percentage="+minusPercentage,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

          function deleteRow(id) {
              console.log("!!!!" + id);
              event.preventDefault();
              console.log("!!!!" + id);
                var getUrl = window.location;
                var storeID =<%=ViewData["storeID"]%>;
                console.log("1");
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                   type: "GET",
                    url: baseUrl+"/api/store/removeDiscount?discountID=" + id +"&storeID=" + storeID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {

                            location.reload();
                        }
                        else {
                            console.log(response);   
                        }
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
          }

          
       function addDiscount(storeID) {         
               event.preventDefault();
               var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
               window.location.href = baseUrl+"/StoreDiscount?storeId=" + storeID;
       }

    </script>
                          
</asp:Content>