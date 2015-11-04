"use strict";

app.controller('AdminSlideController',
    function ($scope, $rootScope, toastr, slides, AdminSlideService, CommmonService,
        DTOptionsBuilder, DTColumnDefBuilder, fileReader) {
        ////Todo here.
        $scope.ListSlides = slides.data.Data;
        $scope.IsNew = false;
        //$scope.NewCategory = {
        //    Name: null,
        //    Description: null
        //};

        //$scope.EditIndex = null;

        // Define table
        $scope.dtOptions = DTOptionsBuilder.newOptions()
        .withDisplayLength(10)
        .withOption('order', [0, 'asc'])
        .withOption('stateSave', true)
        .withBootstrap();

        $scope.dtColumnDefs = [
            DTColumnDefBuilder.newColumnDef(5).notSortable(),
            DTColumnDefBuilder.newColumnDef(6).notSortable()
        ];

        //Clean file input
        function CleanFileInput() {
            angular.forEach(
                angular.element("input[type='file']"),
                    function (inputElem) {
                        angular.element(inputElem).val(null);
                    }
            );

            $scope.file = null;
        }

        // Preview image file
        $scope.getFile = function (file) {
            $scope.file = file;
            fileReader.readAsDataUrl($scope.file, $scope)
                          .then(function (result) {
                              if ($scope.IsNew === true) {
                                  $scope.NewSlide.ImageUrl = result;
                              } else {
                                  $scope.EditSlide.ImageUrl = result;
                              }
                          });
        };
        // Embed video story url
        function getYoutubeUrl(url) {
            var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]{11,11}).*/;
            var match = url.match(regExp);
            if (match)
                if (match.length >= 2) {
                    url = "https://www.youtube.com/embed/" + match[2];
                }
            return url;
        };
        // Submit User model to edit user information
        $scope.addNewSlide = function () {
            $scope.NewSlide.VideoUrl = getYoutubeUrl($scope.NewSlide.VideoUrl);
            var promisePost = AdminSlideService.addSlide($scope.NewSlide, $scope.file);

            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        toastr.success('Tạo mới slide thành công');
                        $scope.ListSlides.push(result.data.Data);
                        $('#newSliderModal').modal('hide');
                    } else if (result.data.Status === "error") {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        $scope.changeSlideStatus = function (id, index) {
            var promise = AdminSlideService.changeSlideStatus(id);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListSlides[index].IsActive = result.data.Data.IsActive;
                        if ($scope.ListSlides[index].IsActive) {
                            toastr.success("Đã mở khóa");
                        } else {
                            toastr.success("Đã khóa lại");
                        }
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        // Functions reset new message form
        $scope.resetNewSlideForm = function resetNewSlideModel(NewSlideForm) {
            $scope.NewSlide = {
                Title: null,
                Description: null,
                ImageUrl: null,
                SlideUrl: null,
                ButtonColor: 'btn-success',
                ButtonText: null,
                TextColor: 'light',
                VideoUrl: null
            };
            $scope.IsNew = true;
            CleanFileInput();
            NewSlideForm.$setPristine(true);
        }

        // Delete slide
        $scope.deleteSlide = function (id, index) {
            var promise = AdminSlideService.deleteSlide(id);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListSlides = result.data.Data;
                        toastr.success("Xóa thành công");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error('Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        // Show edit dialog, mapping data
        $scope.showEditDialog = function (index, EditSlideForm) {
            $scope.EditIndex = index;
            CleanFileInput();
            $scope.IsNew = false;
            var slide = $scope.ListSlides[index];

            $scope.EditSlide = {
                Title: slide.Title,
                Description: slide.Description,
                SlideID: slide.SlideID,
                ImageUrl: "../images/slides/" + slide.ImageUrl,
                SlideUrl: slide.SlideUrl,
                ButtonColor: slide.ButtonColor,
                ButtonText: slide.ButtonText,
                TextColor: slide.TextColor,
                VideoUrl: slide.VideoUrl
            }

            EditSlideForm.$setPristine(true);

            $("#EditSliderModal").modal('show');

        }

        // Edit slide
        $scope.editSlide = function () {
            $scope.EditSlide.VideoUrl = getYoutubeUrl($scope.EditSlide.VideoUrl);
            var promisePost = AdminSlideService.editSlide($scope.EditSlide, $scope.file);
            promisePost.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListSlides[$scope.EditIndex] = result.data.Data;
                        toastr.success("Sửa Slide thành công");
                        $('#EditSliderModal').modal('hide');
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error('Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

        // Edit slide
        $scope.changeOrder = function (index, type) {
            //var temp;
            //var temp2;
            //var tempOrder;
            //if (type === 'up') {
            //    temp = $scope.ListSlides[index];
            //    temp2 = $scope.ListSlides[index - 1];
            //    tempOrder = $scope.ListSlides[index].Order;
            //    $scope.ListSlides[index].Order = temp2.Order;
            //    $scope.ListSlides[index - 1].Order = tempOrder;
            //    $scope.ListSlides[index] = temp2;
            //    $scope.ListSlides[index - 1] = temp;
            //} else {
            //    temp = $scope.ListSlides[index];
            //    temp2 = $scope.ListSlides[index + 1];
            //    tempOrder = $scope.ListSlides[index].Order;
            //    $scope.ListSlides[index].Order = temp2.Order;
            //    $scope.ListSlides[index + 1].Order = tempOrder;
            //    $scope.ListSlides[index] = temp2;
            //    $scope.ListSlides[index + 1] = temp;
            //}
            var promise = AdminSlideService.changeOrder($scope.ListSlides[index].SlideID, type);
            promise.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ListSlides = result.data.Data;
                        toastr.success("Đổi thứ tự thành công");
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        toastr.error('Lỗi');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        }

    });