<%@ Page  Title="Edit Stores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditStore.aspx.cs"  Inherits="WebApplication18.Views.Pages.EditStore" %>

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
             var doc = document.getElementById('allStores');
         var getUrl = window.location;
               var baseUrl = getUrl.protocol + "//" + getUrl.host
            jQuery.ajax({
                type: "GET",
                url: baseUrl+"/api/user/getAllStores",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    console.log(response);
                    if (response !== "fail") {
                        var responseSplit = response.split(';');
                        var HTML = "";
                        for (i = 0; i < responseSplit.length - 1; i++) {
                            split2 = responseSplit[i].split(',');
                            var storeId = split2[0];
                            var storeName = split2[1];

                            HTML  += "<a href=\""+baseUrl+"/Store?storeId="+storeId+"\" class=\"block2-btn flex-c-m stext-103 cl2 size-102 bg0 bor2 hov-btn1 p-lr-15 trans-04 js-show-modal1\" >View Store</a>"; 
                         
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



