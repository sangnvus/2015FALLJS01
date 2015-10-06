﻿"use strict";

app.controller('MessageDetailController', function ($scope, conversation, toastr, MessageService, CommmonService) {
    //Todo here.
    $scope.Conversation = conversation.data.Data;

    $scope.Reply = {
        ConversationID: $scope.Conversation.ConversationID,
        Content: ""
    }

    $scope.ReplyMessage = function () {
        if ($scope.Reply.Content.trim() !== "") {
            var promisePut = MessageService.Reply($scope.Reply);
            promisePut.then(
                function(result) {
                    if (result.data.Status === "success") {
                        $scope.Conversation.MessageList.push(result.data.Data);
                        $scope.Reply.Content = "";
                        toastr.success('Bạn đã gửi tin nhắn thành công!', 'Thành công!');
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function(error) {
                    $scope.Error = error.data.Message;
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo!');
        }
    }
});