<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Route.Home" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
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
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
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
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
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
                </div>
            </div>
        </nav>
        <hr />

        <%--progress bar--%>
        <div id="loadingOverlay" style="display: none; z-index: 9999;">
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        </div>

        <div class="container body-content">
            <h2 style="text-align: center; margin-top: 20px;">Welcome</h2>

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
            <br />


            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>

    <%--scri[pt for progressbar--%>
    <script>
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'block';
        }

        window.onload = function () {
            document.getElementById('loadingOverlay').style.display = 'none';
        };
    </script>

</body>
</html>

