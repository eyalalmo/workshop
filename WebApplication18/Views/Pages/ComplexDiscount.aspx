<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComplexDiscount.aspx.cs" Inherits="WebApplication18.Views.Pages.ComplexDiscount" %>


<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
   <div class="container">
            <div id="Discounts" class ="col-md-12">  
            </div>
          <div id="t" class="pull-right" style = "margin: 10px"><input type="text" class="form-control" id="type" name="type"></div>
         <div class="pull-right" style = "margin: 10px"><h5><strong>Type</strong> <small>(and/or/xor)</small></h5></div>
        <div id="p" class="pull-right" style = "margin: 10px"><input type="text" class="form-control" id="percentage" name="percentage"></div>
         <div class="pull-right" style = "margin: 10px"><h5><strong>Percentage</strong></h5></div>
       <div id="d" class="pull-right" style = "margin: 10px"><input type="text" class="form-control" id="duration" name="duration"></div>
         <div class="pull-right" style = "margin: 10px"><h5><strong>Duration</strong></h5></div>
        <div id="makeComplex" class ="col-md-12">
            </div>   
        </div>
      <script type="text/javascript">

        $(document).ready(function () {
            
            var mainDiv = document.getElementById('Discounts');
            var makeComplexDiv = document.getElementById('makeComplex');

               
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

                                var str ="<table class =\"table table-bordered text-center\"><tbody>";
                                var discounts = response.split(";");
                                for (i = 0; i < discounts.length - 1; i++) {
                                    var discountfields = discounts[i].split(",");
                                    var type = discountfields[0];
                                    var description = discountfields[1];
                                    var percentage = discountfields[2];
                                    var duration = discountfields[3];
                                    var discountID = discountfields[4];
                                    str += "<tr>" +
                                        "<td  style = \"width:30px\"><input type=\"checkbox\"></td><td style = \"width:450px\" align=\"left\"> <h4 class=\"discount\"><strong>" + type + "</strong></h4><h4><small>" + description + "</small></h4></td><td style = \"width:120px\;top:100px; align=\"center\">"+duration+"</td><td style = \"width:50px\"> <div class=\"quantity\"><input type=\"button\" value=\"+\" onclick=\"plusQuantity(" + discountID + "," + percentage+");\" class=\"plus\"><input type=\"text\" value=\"" + percentage + " %\" title=\"Qty\" class=\"qty\"><input type=\"button\" value=\"-\" onclick=\"minusQuantity(" + discountID + "," + percentage+");\" class=\"minus\"></div></td><td style = \"width:50px\" align=\"center\"> <button type=\"button\" class=\"btn btn-danger\" onclick=\"deleteRow("+discountID+");\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></button></td></tr>";
                                }
                                str += " </tbody>" + "</table>";
                                console.log(str);
                                mainDiv.innerHTML = str;
                                makeComplexDiv.innerHTML = "<div class=\"pull-right\" style = \"margin: 10px\" ><input type=\"button\" class=\"btn btn-secondary\" value=\"Make Complex\" onclick=\" makeComplex(" + storeID + ")\"></div>";

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

          //<td><div class=\"col-4 col-sm-4 col-md-4 text-right\"><input type=\"button\"  class=\"btn btn-secondary\" value=\"Complex\" onclick=\"complex();\"></div></td>

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
              event.preventDefault();
                var getUrl = window.location;
                var storeID =<%=ViewData["storeID"]%>;
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
          
          function makeComplex() {
              event.preventDefault();
                var getUrl = window.location;
              var storeID =<%=ViewData["storeID"]%>;
              var s = "";
              type = $("#type").val();
              percentage = $("#percentage").val();
              duration = $("#duration").val();

              $('table [type="checkbox"]').each(function(i, chk) {
                    if (chk.checked) {
                        console.log("Checked!", i, chk);
                        s = s + i + " ";
                    }
                  });
              console.log("type::::" + type);
              var baseUrl = getUrl.protocol + "//" + getUrl.host
               jQuery.ajax({
                   type: "GET",
                   url: baseUrl + "/api/store/complexDiscount?discounts=" + s + "&storeID=" + storeID + "&type=" + type + "&percentage=" + percentage+ "&duration=" + duration,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response == "ok") {
                            event.preventDefault();
                            var getUrl = window.location;
                            var baseUrl = getUrl.protocol + "//" + getUrl.host
                            window.location.href = baseUrl+"/StoreDiscounts?storeId=" + storeID;
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
           
         

    </script>
                          
</asp:Content>