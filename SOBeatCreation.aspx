<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SOBeatCreation.aspx.cs" Inherits="Route.SOBeatCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SO Beat Creation</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
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

        /* Container for the progress bar */
        .progress-bar-container {
            width: 100%; /* Makes the container take up the full width */
            /*max-width: 600px;*/ /* Optional: sets a maximum width for larger screens */
            background-color: #e0e0e0;
            border-radius: 0px;
            overflow: hidden;
            position: relative;
            margin-top: -20px; /* Adjusts spacing from the <hr /> */
            margin-bottom: 20px; /* Optional: adds some space after the progress bar */
        }

        /* The animated progress bar */
        .progress-bar {
            height: 5px;
            background: linear-gradient(to right, #4caf50, #81c784, #4caf50); /* Green gradient */
            width: 0%; /* Starts from 0% width */
            animation: progress-animation 2s infinite; /* Animation for progress effect */
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Style the autocomplete dropdown to look like a DropDownList */
        .ui-autocomplete {
            list-style: none;
            margin: 0;
            padding: 0;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            max-height: 200px;
            overflow-y: auto;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            z-index: 1050;
            width: 50px; /* Match the width of the input field */
        }

        .ui-menu-item {
            padding: 8px 12px;
            font-size: 14px;
        }

            .ui-menu-item:hover {
                background-color: #007bff;
                color: white;
            }

        /* Hide any default message shown by jQuery UI */
        .ui-helper-hidden-accessible {
            display: none;
        }

        /* Navbar */
        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        /* Sidebar */
        .sidebar {
            position: fixed;
            top: 50px;
            left: -250px;
            width: 250px;
            height: calc(100vh - 50px);
            /*background-color: rgba(34, 34, 34, 0.95);*/
            color: white;
            overflow-y: auto;
            transition: left 0.3s ease-in-out;
            z-index: 1100;
            padding: 15px;
        }

            .sidebar.open {
                left: 0;
            }

            .sidebar ul {
                padding: 0;
                list-style: none;
            }

            .sidebar .nav-item {
                padding: 8px 0;
            }

            .sidebar .nav-link {
                color: white;
                text-decoration: none;
                display: block;
                padding: 10px;
                transition: background 0.3s;
            }

                .sidebar .nav-link:hover {
                    background: rgba(255, 255, 255, 0.2);
                }

        /* Overlay */
        .overlay {
            display: none;
            position: fixed;
            top: 50px;
            left: 0;
            width: 100%;
            height: calc(100vh - 50px);
            background: rgba(0, 0, 0, 0.5);
            z-index: 1050;
        }

        /* Content */
        .content {
            transition: opacity 0.3s ease-in-out;
        }

        .sidebar.open ~ .content {
            opacity: 0.7;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
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
    </script>

    <script>
        function toggleSidebar() {
            $(".sidebar").toggleClass("open");
            $(".overlay").toggle();
        }

        $(document).ready(function () {
            $(".overlay").click(function () {
                $(".sidebar").removeClass("open");
                $(this).hide();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="overlay"></div>

        <%--<nav class="navbar navbar-expand-lg navbar-white bg-white">
            <div class="container">
                <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav ml-auto">
                        <li class="nav-item">
                            <a id="HomeLink" class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                        </li>
                        <li class="nav-item">
                            <a id="CreateLink" class="nav-link" runat="server" href="~/Create" onclick="showLoading()">Create</a>
                        </li>
                        <li class="nav-item">
                            <a id="ModifyLink" class="nav-link" runat="server" href="~/Map" onclick="showLoading()">Map</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/Transfer" onclick="showLoading()">Transfer</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/NewGeo" onclick="showLoading()">NewGeo</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/BeatReailgnment" onclick="showLoading()">BeatReailgnment</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" runat="server" href="~/SOBeatCreation" onclick="showLoading()">SOBeatCreation</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>--%>

        <!-- Navbar -->
        <nav class="navbar navbar-white bg-white">
            <button class="navbar-toggler" type="button" onclick="toggleSidebar()">
                <span class="navbar-toggler-icon"></span>
            </button>
            <%--<span class="brand">SYNAPSE</span>--%>
            <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
        </nav>
        <hr />

        <%--progress bar--%>
        <div id="loadingOverlay" style="display: none; z-index: 9999;">
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        </div>

        <div class="container body-content">

            <aside class="sidebar">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Create" onclick="showLoading()">Create</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Map" onclick="showLoading()">Map</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Transfer" onclick="showLoading()">Transfer</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/NewGeo" onclick="showLoading()">NewGeo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/BeatReailgnment" onclick="showLoading()">BeatReailgnment</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/SOBeatCreation" onclick="showLoading()">SOBeatCreation</a>
                    </li>
                </ul>
            </aside>

            <!-- Main Content -->
            <main class="content">
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
                <h2 style="text-align: center; margin-top: 20px;">SO Beat Creation</h2>
                <br />

                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-12 col-md-3 mb-2 mb-md-0">
                            <asp:DropDownList ID="ZoneDrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="ZoneDrp_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-3 mb-2 mb-md-0">
                            <asp:DropDownList ID="SODrp" runat="server" AutoPostBack="true" class="form-control" onchange="showLoading()" OnSelectedIndexChanged="SODrp_SelectedIndexChanged">
                                <asp:ListItem Text="SO Code" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-12 col-md-3 mb-2 mb-md-0">
                            <asp:Button ID="Submit" runat="server" Text="Create" CssClass="btn btn-success form-control" OnClientClick="showLoading()" OnClick="Submit_Click" />
                        </div>

                    </div>
                    <div class="col-12 col-md-3 mb-2 mb-md-0">
                        <button type="button" class="form-control" id="Button1" runat="server" data-toggle="modal" data-target="#resultModalCenter" style="display: none;">
                            Modal                       
                        </button>
                    </div>

                    <div class="row mt-3">
                        <div class="col-12">
                            <div class="grid-wrapper">
                                <asp:GridView ID="DistRtrLoadGrid2" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                    Style="margin-bottom: 0px; text-align: center;">
                                    <Columns>
                                        <asp:BoundField DataField="DistCode" HeaderText="DistCode" />
                                        <asp:BoundField DataField="RtrCode" HeaderText="Retailer Code" />
                                        <asp:BoundField DataField="URCode" HeaderText="UR Code" />

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div style="margin-right: 10px;">
                                                    <input type="checkbox" id="CheckBox1" runat="server" class="form-check-input" style="position: relative; margin-left: -3px;" checked="checked" disabled="disabled" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>

                <%-- Modal for Result --%>
                <div class="modal fade" id="resultModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="resultModalLongTitle">Uploadation failed data</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>

                            <div class="modal-body" style="max-height: 400px; overflow-y: auto;">
                                <div class="form-group">
                                    <asp:GridView ID="ResultModalGrid" runat="server" AutoPostBack="True" CssClass="table table-bordered form-group"
                                        AutoGenerateColumns="false" DataKeyNames="" Style="margin-bottom: -18px; text-align: center">
                                        <Columns>
                                            <asp:BoundField DataField="Distcode" HeaderText="Dist. Code" />
                                            <asp:BoundField DataField="RouteCode" HeaderText="Route Code" />
                                            <asp:BoundField DataField="RouteName" HeaderText="Route Name" />
                                            <asp:BoundField DataField="UniqueRouteCode" HeaderText="UniqueRouteCode" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" />

                                        </Columns>
                                        <HeaderStyle CssClass="header-hidden" />
                                        <RowStyle CssClass="fixed-height-row" BackColor="#FFFFFF" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <%--<button type="button" class="btn btn-primary" onclick="selectItems()" data-dismiss="modal">Select</button>--%>
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>

    <%-- Script for selectall checkboxes in Modal --%>
    <script type="text/javascript">
        function selectAllCheckboxes(source) {
            var checkboxes = document.querySelectorAll('.rowCheckbox');
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = source.checked;
            }
        }
    </script>

    <%-- Script for search button in Modal --%>
    <%-- <script type="text/javascript">
        $(document).ready(function () {
            $("#txtSearch").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#<%= DayId.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>--%>

    <%--Script to allow only alphanumeric characters, space, and underscore --%>
    <script type="text/javascript">
        // Function to allow only alphanumeric characters, space, and underscore
        function isValidInput(event) {
            var charCode = event.which || event.keyCode;
            var charStr = String.fromCharCode(charCode);

            // Allow backspace, delete, tab, escape, enter, space, underscore, and alphanumerics
            if (charCode === 8 || charCode === 9 || charCode === 13 || charCode === 27 ||
                charCode === 32 || charStr === "_" || /^[a-zA-Z0-9]$/.test(charStr)) {
                return true;
            }

            return false;
        }

        // Function to remove any invalid characters if pasted into the textbox
        function removeInvalidChars(input) {
            input.value = input.value.replace(/[^a-zA-Z0-9 _]/g, '');
        }
    </script>

    <%--script for progressbar--%>
    <script>
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'block';
        }

        window.onload = function () {
            document.getElementById('loadingOverlay').style.display = 'none';
        };
    </script>

    <%--Script for Dist Dropdown Auto Search--%>
    <%--<script>
        $(document).ready(function () {
            // Get DropDownList options and convert to an array
            var options = [];
            $('#<%= DistDrp.ClientID %> option').each(function () {
                var text = $(this).text();  // DistNm
                var value = $(this).val();  // DistCode
                if (value) {
                    options.push({ label: text, value: value });
                }
            });

            // Initialize jQuery UI Autocomplete
            $('#DistSearch').autocomplete({
                source: options,
                select: function (event, ui) {
                    // Set the selected value in the input box (DistNm)
                    $('#DistSearch').val(ui.item.label);

                    // Update the hidden DropDownList (DistDrp) value
                    $('#<%= DistDrp.ClientID %>').val(ui.item.value);

                    // Trigger OnSelectedIndexChanged event of DropDownList
                    __doPostBack('<%= DistDrp.UniqueID %>', '');

                    showLoading();

                    // Return true to avoid clearing the selected value
                    return true; // Ensures value remains in the input field
                }
            });
        });
    </script>--%>
</body>
</html>
