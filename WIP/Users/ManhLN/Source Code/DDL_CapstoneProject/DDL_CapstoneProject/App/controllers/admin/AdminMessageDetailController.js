"use strict";

app.controller('AdminMessageDetailController', function ($scope, $location, $sce, $rootScope,
    conversation, toastr, MessageService, CommmonService, SweetAlert) {
    //Todo here.
    $scope.Conversation = conversation.data.Data;
    $scope.CurrentUser = $rootScope.UserInfo;
    $scope.NumberNewMessage = $rootScope.NumberNewMessage;
    $scope.Reply = {
        ConversationID: $scope.Conversation.ConversationID,
        Content: ""
    }

    // Get message number.
    function getNewMessageNumber() {
        if ($scope.NumberNewMessage === undefined) {
            var promiseGet = MessageService.getNumberNewMessage();
            promiseGet.then(
                function (result) {
                    if (result.data.Status === "success") {
                        //Save new message number into $rootScope
                        $scope.NumberNewMessage = result.data.Data;
                        $scope.NumberNewMessage.Total = result.data.Data.ReceivedMessage + result.data.Data.SentMessage;
                    } else {
                        $scope.NumberNewMessage.Total = 0;
                        $scope.NumberNewMessage = 0;
                        $scope.NumberNewMessage = 0;
                    }
                },
                function (error) {
                    $scope.NumberNewMessage = 0;
                    $scope.NumberNewMessage = 0;
                    $scope.NumberNewMessage = 0;
                });
        }
    }

    getNewMessageNumber();

    // Function check string startwith 'http'
    $scope.checkHTTP = function (input) {
        var lowerStr = (input + "").toLowerCase();
        return lowerStr.indexOf('http') === 0;
    }

    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // function reply a conversation
    $scope.ReplyMessage = function () {
        if ($scope.Reply.Content.trim() !== "") {
            var promisePut = MessageService.Reply($scope.Reply);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Conversation.MessageList.push(result.data.Data);
                        $scope.Reply.Content = "";
                        toastr.success('Trả lời thành công');
                    } else {
                        var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        if (a) {
                            $scope.Error = result.data.Message;
                            toastr.error($scope.Error, 'Lỗi');
                        }
                    }
                },
                function (error) {
                    toastr.error('Lỗi!');
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo');
        }
    }

    // function to delete message.
    $scope.Delete = function () {
        SweetAlert.swal({
            title: "Xóa tin nhắn",
            text: "Bạn có chắc chắn muốn xóa tin nhắn này không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có, tôi chắc chắn",
            cancelButtonText: "Không",
            closeOnConfirm: true
        },
                   function (isConfirm) {
                       if (isConfirm) {
                           var promise = MessageService.Delete($scope.Conversation.ConversationID);
                           promise.then(
                               function (result) {
                                   if (result.data.Status === "success") {
                                       $location.path("/message").replace();
                                   } else {
                                       var a = CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                                       if (a) {
                                           $scope.Error = result.data.Message;
                                           toastr.error($scope.Error, 'Lỗi');
                                       }
                                   }
                               },
                               function (error) {
                                   toastr.error('Lỗi');
                               });
                       }
                   });
    }
});