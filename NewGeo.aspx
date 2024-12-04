<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewGeo.aspx.cs" Inherits="Route.NewGeo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Geo</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }

            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Spinner styles */
        .spinner {
            width: 30px;
            height: 30px;
            border: 4px solid #ddd;
            border-top: 4px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: auto;
            margin-left: 0px;
            margin-top: 5px;
            display: none; /* Hidden by default */
        }

        /* Keyframes for spinner animation */
        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* Style for the file upload container */
        .file-upload-container {
            position: relative;
        }

        /* Style for the file input button */
        .file-upload-input {
            padding-right: 50px; /* Space for the image */
        }

        /* Style for the link that contains the image */
        .file-upload-link {
            position: absolute;
            top: 50%;
            right: 0px; /* Adjust position of the icon */
            transform: translateY(-50%);
            background-color: #e6e6e6; /* Same as the button color */
            border-radius: 3px;
            padding: 5px;
            display: inline-block;
        }

        /* Style for the Excel image */
        .file-upload-icon {
            width: 25px; /* Adjust the size of the image */
            height: 25px;
            background-color: transparent; /* Keep the background transparent */
        }

    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }

        function showLoader() {
            document.getElementById('spinner').style.display = 'block';
        }

        // Function to hide loader
        function hideLoader() {
            document.getElementById('spinner').style.display = 'none';
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">

        <nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a id="HomeLink" class="nav-link" runat="server" href="~/Home">Home</a>
                        </li>
                        <li class="nav-item">
                            <a id="CreateLink" class="nav-link" runat="server" href="~/Create">Create</a>
                        </li>
                        <li class="nav-item">
                            <a id="ModifyLink" class="nav-link" runat="server" href="~/Map">Map</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Transfer">Transfer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/NewGeo">NewGeo</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>

        <hr />

        <div class="container body-content">
            <div class="headtag">
                <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
            </div>
            <table style="width: 100%; font-family: Calibri; font-size: small">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lbl_msg" Text="" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large" BackColor="LightPink"></asp:Label>
                    </td>
                </tr>
            </table>
            <h2 style="text-align: center; margin-top: 20px;">New Geo</h2>
            <br />

            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <asp:TextBox ID="DistCodeTxt" runat="server" CssClass="form-control" placeholder="Dist. Code"></asp:TextBox>
                    </div>
                    <div id="btnDivSingle" class="col-12 col-md-3 mb-2 mb-md-0" runat="server" visible="true">
                        <asp:Button ID="EnterSubmit" runat="server" Text="Enter" CssClass="btn btn-primary form-control"
                            OnClick="BtnEnter_Click" />
                    </div>
                    <div id="btnDivSplit" class="col-12 col-md-3 mb-2 mb-md-0" runat="server" visible="false">
                        <div class="file-upload-container position-relative">
                            <asp:FileUpload ID="FileUpload_Id" runat="server" CssClass="form-control file-upload-input" accept=".xls, .xlsx, .xlsb" />
                            <a href="Excel/Sample.xlsx" download="Sample" class="file-upload-link" title="Download Sample Excel Template">
                                <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Microsoft_Office_Excel_%282019%E2%80%93present%29.svg/512px-Microsoft_Office_Excel_%282019%E2%80%93present%29.svg.png?20190925171014" alt="Excel Logo" class="file-upload-icon" /></a>
                        </div>
                    </div>
                    <div id="btnDivSplit2" class="col-12 col-md-3 mb-2 mb-md-0" runat="server" visible="false">
                        <asp:Button ID="SubmitBtn" runat="server" Text="Submit" CssClass="btn btn-info form-control" OnClick="Submit_Click" />
                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <div id="spinner" class="spinner"></div>
                    </div>
                </div>
            </div>

            <%-- Note --%>
            <div class="container">
                <div class="row mt-3">
                    <div class="alert alert-success" role="alert" id="noteAlert" style="display: none;">
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <h4 class="alert-heading">Note</h4>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <%--<span aria-hidden="true">&times;</span>--%>
                            </button>
                        </div>
                        <hr />
                        <p class="mb-0">
                            Some of the 'OUTLET CLASSIFICATION 1' will be replaced with the marked 'ExistingData'. 
                            Click 
                            <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="btn btn-success form-control" Width="100px" OnClick="btnContinue_Click" />
                            to proceed.
                        </p>
                    </div>
                </div>
            </div>

            <%-- GridView --%>
            <div class="row">
                <div class="col-12">
                    <div class="grid-wrapper">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                            Style="margin-bottom: 0px; text-align: center;" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="RETAILER CODE" HeaderText="RETAILER CODE" />
                                <asp:BoundField DataField="RETAILER NAME" HeaderText="RETAILER NAME" />
                                <asp:BoundField DataField="ROUTE CODE" HeaderText="ROUTE CODE" />
                                <asp:BoundField DataField="ROUTE NAME" HeaderText="ROUTE NAME" />
                                <asp:BoundField DataField="OUTLET CLASSIFICATION 2" HeaderText="OUTLET CLASSIFICATION 2" />
                                <asp:BoundField DataField="OUTLET CLASSIFICATION 3" HeaderText="OUTLET CLASSIFICATION 3" />
                                <asp:BoundField DataField="OUTLET CLASSIFICATION 1" HeaderText="OUTLET CLASSIFICATION 1" />
                                <asp:BoundField DataField="ExistingData" HeaderText="ExistingData" />

                                <asp:BoundField DataField="MatchStatus" HeaderText="MatchStatus" Visible="false" />

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>


            <%--<asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />--%>


            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>

    <%-- Script for Loading --%>
    <script>
        $('#<%= SubmitBtn.ClientID %>').click(function () {
            showLoader();
        });

        $('#<%= btnContinue.ClientID %>').click(function () {
            showLoader();
        });
    </script>

</body>
</html>

