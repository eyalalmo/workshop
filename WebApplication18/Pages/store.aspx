<%@ Page Title="Store" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="store.aspx.cs" Inherits="WebApplication18.Pages.store" %>

<asp:Content ID="BodyContent"  ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>
    </h2>
     <%--<div class="bg0 m-t-23 p-b-140" style="margin-left: auto; margin-right: auto; margin-top: 45px; max-width: 100%;">
        <div class="container">
            <div id="allStores" class="row isotope-grid" style="position: relative;">
            </div>
        </div>
    </div>--%>

    <div class="container">
	<div id="allStores" class="row">
        </div>
		</div>

    <script type="text/javascript">
        $(document).ready(function () {
            var storeId = @DataView["StoreId"];
             var doc = document.getElementById('allStores');
         var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/store/getStoreProducts?storeId=" + storeId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response);
                    if (response !== "fail") {
                        var responseSplit = response.split(';');
                        var HTML = "";
                        for (i = 0; i < responseSplit.length - 1; i++) {
                            split2 = responseSplit[i].split(',');
                            var productId = split2[0];
                            var ProductName = split2[1];

                            HTML += `<div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
		                   <div class="my-list">
			               <h3><div id=\"storeName" + i + "\"Product Name: `+ ProductName+ ` </div></h3>
                            <a href=\"" + baseUrl + "/store?storeId=" `+ productId +` "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Product</a>   
		                    </div>
		                     </div>`
                            //var string = "";
                            //string += "<div class=\"col-sm-6 col-md-4 col-lg-3 p-b-35 isotope-item women\" >";
                            //string += "<div class=\"block2\">";

                            //string += "<a href=\"" + baseUrl + "/store?storeId=" + storeId + "\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store</a>";
                            //string += "</div>";
                            //string += "<div class=\"block2-txt flex-w flex-t p-t-14\" style=\"width: 270px;padding: 17px;border-color: black;border-width: 1px;border-style: groove;margin-left:20px; margin-bottom: 20px;\">";
                            //string += "<div class=\"block2-txt-child1 flex-col-l \">";
                            //string += "<div id=\"storeName" + i + "\">Store Name: " + storeName + "</div>";
                            //string += "</a>";
                            //string += "<span class=\"stext-105 cl3\">";

                            //string += "</span>";
                            //string += "</div>";
                            //string += "</div>";
                            //string += "</div>";
                            //string += "</div>";
                            //mainDiv.innerHTML += string;

                        }
                        doc.innerHTML += HTML;;
                        
                    }
                    else {
                        alert("problem");
                    }
                },
                error: function (response) {
                    //alert("Lost DB connection");
                    window.location.href = baseUrl+"/index";
                }
            });
        });
    </script>
 </asp:Content>


