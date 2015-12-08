"use strict";

app.controller("EditProjectController", function ($scope, $filter, $rootScope, $sce, $location, toastr, CommmonService, $routeParams, ProjectService, categories, project, fileReader, SweetAlert) {
    // initial newReward
    $scope.NewReward = {};
    // initial newUpdateLog
    $scope.NewUpdateLog = {};
    // initial newTimeline
    $scope.newTimeline = {};
    // initial newQuestion
    $scope.newQuestion = {};
    // initial edit timeline status list
    $scope.editTimeline = {};
    // initail editRewardPkg
    $scope.editPkg = {};
    // initail reward type
    $scope.rewardTypes = [
        { name: 'Không quà tặng', value: "no reward" },
        { name: 'Giới hạn', value: "limited" },
        { name: 'Không giới hạn', value: "unlimited" },
    ];
    // Detech create a new timeline point
    $scope.isCreateTimeline = false;

    // check error list
    $scope.errorListFlag = false;
    // check create reward
    $scope.newRewardError = false;
    // check edit updatelog
    $scope.IsEditUpdateLog = [];
    // check edit QA
    $scope.IsEditQA = [];

    // Check first load
    $scope.FirstLoadReward = false;
    $scope.FirstLoadUpdate = false;
    $scope.FirstLoadQA = false;
    $scope.FirstLoadTimeLine = false;
    $scope.FirstLoadStory = false;

    // Get current time
    $scope.toDay = new Date($.now());
    $scope.NewReward.EstimatedDelivery = angular.copy($scope.toDay);

    // Check current time of update log
    var dd = $scope.toDay.getDate();
    var mm = $scope.toDay.getMonth() + 1; //January is 0!
    var yyyy = $scope.toDay.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    }

    $scope.checkUpdateLogDate = mm + '/' + dd + '/' + yyyy;

    // Set min deadline : current time + 10 days
    var minDate = new Date(new Date().getTime() + (11 * 24 * 60 * 60 * 1000));
    var minDateDD = minDate.getDate();
    var minDateMM = minDate.getMonth() + 1;
    var minDateYY = minDate.getFullYear();

    if (parseInt(minDateDD) < 10) {
        minDateDD = "0" + minDateDD;
    }

    if (parseInt(minDateMM) < 10) {
        minDateMM = "0" + minDateMM;
    }

    $("#ProjectDeadLine").attr({
        "min": minDateYY + "-" + minDateMM + "-" + minDateDD
    });

    $scope.endDate = minDateYY + "-" + minDateMM + "-" + minDateDD;

    $scope.Project = {};
    $scope.Project.Status = "approved";

    // Get project's basic record
    $scope.Project = project.data.Data;

    // Check project's status
    $scope.AllEditable = true;
    if ($scope.Project.Status === "expired" || $scope.Project.Status === "suspended" || $scope.Project.Status === "pending") {
        $scope.AllEditable = false;
    };

    // Project is approved
    $scope.PrjApprove = false;
    if ($scope.Project.Status === "approved") {
        $scope.PrjApprove = true;
    }

    // Turn URL to trust Src
    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Embed video story url
    $scope.checkVideoURL = function () {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]{11,11}).*/;
        var match = $scope.ProjectStory.VideoUrl.match(regExp);
        if (match) if (match.length >= 2) {
            $scope.ProjectStory.VideoUrl = "https://www.youtube.com/embed/" + match[2];
        }
    };

    // Get categories
    $scope.Categories = categories.data.Data;

    // Checking project categoy
    var categoryIndex = 0;
    $scope.selectedCate = function () {
        for (var i = 0; i < $scope.Categories.length; i++) {
            if ($scope.Categories[i].CategoryID == $scope.Project.CategoryID) {
                $scope.Project.CategoryName = $scope.Categories[i].Name;
                categoryIndex = i;
                break;
            }
        };
    }
    $scope.selectedCate();

    // Set selected project category
    $scope.selectedOption = $scope.Categories[categoryIndex];
    $scope.originalSelectedCate = angular.copy($scope.selectedOption);

    // convert datetime to date
    if ($scope.Project.ExpireDate != null) {
        $scope.originalExpireDate = angular.copy($scope.Project.ExpireDate);
        $scope.Project.ExpireDate = new Date($filter('date')($scope.Project.ExpireDate, "yyyy-MM-dd"));
    }

    // Copy original project's basic
    $scope.originalProjectBasic = angular.copy($scope.Project);

    // Detech Active tab
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $scope.activeTab = $(e.target).attr("href");
    });

    // check file's size is too big
    $scope.fileIsBig = false;

    // Preview image file
    $scope.getFile = function (file, index) {
        $scope.file = file;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          if ($scope.activeTab === "#timelineTab" && $scope.isCreateTimeline === false) {
                              $scope.Timeline[index].ImageUrl = result;
                          } else if ($scope.activeTab === "#timelineTab" && $scope.isCreateTimeline === true) {
                              $scope.newTimeline.ImageUrl = result;
                          } else {
                              $scope.Project.ImageUrl = result;
                          }
                          if ($scope.file.size > 52428800) {
                              $scope.fileIsBig = true;
                          } else {
                              $scope.fileIsBig = false;
                          }
                      });
    };

    $scope.editProjectBasic = function (form) {
        // Update project category
        $scope.Project.CategoryId = $scope.selectedOption.CategoryID;

        // Put update project
        var promisePut = ProjectService.editProject($scope.file, $scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    $scope.Project = result.data.Data;
                    $scope.selectedCate();
                    $scope.selectedOption = $scope.Categories[categoryIndex];
                    $scope.originalSelectedCate = angular.copy($scope.selectedOption);
                    // re-set original project basic
                    $scope.originalProjectBasic = angular.copy($scope.Project);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.file = null;
                    if ($scope.Project.ExpireDate != null) {
                        $scope.originalExpireDate = angular.copy($scope.Project.ExpireDate);
                    }
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Get project's story
    $scope.getStory = function () {
        if (!$scope.FirstLoadStory) {
            var promiseGetProjectStory = ProjectService.getProjectStory($scope.Project.ProjectID);

            promiseGetProjectStory.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.ProjectStory = result.data.Data;
                        // Copy original project's basic
                        $scope.originalStory = angular.copy($scope.ProjectStory);
                        $scope.FirstLoadStory = true;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };

    // Edit project's story
    $scope.editProjectStory = function (form) {
        // Put update project's story
        var promisePutProjectStory = ProjectService.editProjectStory($scope.ProjectStory);

        promisePutProjectStory.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    // re-set original project story
                    $scope.originalStory = angular.copy($scope.ProjectStory);
                    form.$setPristine();
                    form.$setUntouched();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Get rewardPkgs record
    $scope.getReward = function () {
        if (!$scope.FirstLoadReward) {
            var promiseGetReward = ProjectService.getRewardPkgs($scope.Project.ProjectID);

            promiseGetReward.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.RewardPKgs = result.data.Data;
                        // convert datetime to date
                        for (var i = 0; i < $scope.RewardPKgs.length; i++) {
                            if ($scope.RewardPKgs[i].EstimatedDelivery != null) {
                                $scope.RewardPKgs[i].EstimatedDelivery = new Date($filter('date')($scope.RewardPKgs[i].EstimatedDelivery, "yyyy-MM-dd"));
                            }
                        };
                        // Copy original project reward
                        $scope.originalReward = angular.copy($scope.RewardPKgs);
                        $scope.FirstLoadReward = true;

                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };
    // Edit rewardPkg
    $scope.updateReward = function (index) {
        var promiseUpdateReward;
        if (index != null) {
            if ($scope.RewardPKgs[index].IsHide == true) {
                $scope.RewardPKgs[index].IsHide = false;
            } else {
                $scope.RewardPKgs[index].IsHide = true;
            }

            promiseUpdateReward = ProjectService.editRewardPkgs($scope.RewardPKgs[index]);
        } else {
            $scope.editPkg.Type = $scope.selectedType.value;
            if ($scope.editPkg.Type != "limited") {
                $scope.editPkg.Quantity = 0;
            }
            $scope.RewardPKgs[$scope.editPkg.Index - 1] = angular.copy($scope.editPkg);
            promiseUpdateReward = ProjectService.editRewardPkgs($scope.editPkg);
        }

        promiseUpdateReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    $('#editRewardModal').modal('hide');
                    $scope.RewardPKgs.sort(function (a, b) {
                        return parseFloat(a.PledgeAmount) - parseFloat(b.PledgeAmount);
                    });
                    // re-set original project reward
                    $scope.originalReward = angular.copy($scope.RewardPKgs);
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Create a new reward
    $scope.createReward = function () {
        if ($scope.NewReward.PledgeAmount < 10000 || $scope.NewReward.PledgeAmount == '' || $scope.NewReward.PledgeAmount == null
            || $scope.NewReward.Description == null
            || $scope.NewReward.Description.length < 10 || $scope.NewReward.Description.length > 135
            || $scope.NewReward.Description == ''
            || ($scope.NewReward.EstimatedDelivery < $scope.Project.ExpireDate && $scope.Project.ExpireDate != null && $scope.NewReward.Type != "no reward")
            || (($scope.NewReward.Quantity < 1 || $scope.NewReward.Quantity == null || $scope.NewReward.Quantity == '') && $scope.NewReward.Type == "limited")) {
            $scope.newRewardError = true;
        } else {
            $scope.newRewardError = false;
        }


        if ($scope.NewReward.Type != "limited") {
            $scope.NewReward.Quantity = 0;
        }

        if (!$scope.newRewardError) {
            if ($scope.PrjApprove) {
                $scope.NewReward.IsHide = true;
            }
            var promiseCreateReward = ProjectService.createReward($scope.Project.ProjectID, $scope.NewReward);

            promiseCreateReward.then(
                function (result) {
                    if (result.data.Status === "success") {
                        toastr.success('Tạo gói quà thành công!');
                        $('#addReward').modal('hide');
                        // reinitial newReward
                        $scope.NewReward = {};
                        $scope.NewReward.Type = "no reward";
                        result.data.Data.EstimatedDelivery = new Date($filter('date')(result.data.Data.EstimatedDelivery, "yyyy-MM-dd"));
                        if (result.data.Data.Quantity > 0) {
                            result.data.Data.LimitQuantity = true;
                        }
                        $scope.RewardPKgs.push(result.data.Data);
                        $scope.RewardPKgs.sort(function (a, b) {
                            return parseFloat(a.PledgeAmount) - parseFloat(b.PledgeAmount);
                        });
                        $scope.originalReward = angular.copy($scope.RewardPKgs);
                        $scope.NewReward.EstimatedDelivery = angular.copy($scope.toDay);
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };
    // Delete a rewardPkg
    $scope.deleteReward = function (index) {
        SweetAlert.swal({
            title: "Xóa mức ủng hộ!",
            text: "Bạn có chắc chắn muốn xóa không!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có!",
            cancelButtonText: "Không!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    var promiseDeleteReward = ProjectService.deleteRewardPkg($scope.RewardPKgs[index].RewardPkgID);

                    promiseDeleteReward.then(
                        function (result) {
                            if (result.data.Status === "success") {
                                toastr.success('Xóa thành công!');
                                $('#editRewardModal').modal('hide');
                                $scope.RewardPKgs.splice(index, 1);
                                $scope.originalReward = angular.copy($scope.RewardPKgs);
                            } else {
                                CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                                $scope.Error = result.data.Message;
                                toastr.error($scope.Error, 'Lỗi!');
                            }
                        },
                        function (error) {
                            $scope.Error = error.data.Message;
                            toastr.error($scope.Error, 'Lỗi!');
                        });
                } else {
                }
            });
    };
    // Show edit rewardPkg modal
    $scope.editRewardModal = function (index) {
        $('#editRewardModal').modal('show');
        $scope.editPkg = angular.copy($scope.RewardPKgs[index]);
        $scope.editPkg.Index = index + 1;
        for (var i = 0; i < $scope.rewardTypes.length; i++) {
            if ($scope.rewardTypes[i].value === $scope.editPkg.Type) {
                $scope.selectedType = $scope.rewardTypes[i];
                break;
            }
        };
    }

    // Get updateLog records
    $scope.getUpdateLog = function () {
        if (!$scope.FirstLoadUpdate) {
            // Put update project
            var promiseGetUpdateLog = ProjectService.getUpdateLogs($scope.Project.ProjectID);

            promiseGetUpdateLog.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.UpdateLogs = result.data.Data;
                        // convert datetime to date
                        for (var i = 0; i < $scope.UpdateLogs.length; i++) {
                            $scope.UpdateLogs[i].CreatedDate = new Date($filter('date')($scope.UpdateLogs[i].CreatedDate, "yyyy-MM-dd"));
                            var dd = $scope.UpdateLogs[i].CreatedDate.getDate();
                            var mm = $scope.UpdateLogs[i].CreatedDate.getMonth() + 1; //January is 0!
                            var yyyy = $scope.UpdateLogs[i].CreatedDate.getFullYear();

                            if (dd < 10) {
                                dd = '0' + dd;
                            }

                            if (mm < 10) {
                                mm = '0' + mm;
                            }
                            $scope.UpdateLogs[i].CreatedDate = mm + '/' + dd + '/' + yyyy;
                        };
                        // Copy original update log
                        $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                        $scope.FirstLoadUpdate = true;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };
    // Function show edit update log form.
    $scope.showEditUpdateLog = function (index) {
        if ($scope.IsEditUpdateLog[index] == null || $scope.IsEditUpdateLog[index] === false) {
            $scope.IsEditUpdateLog[index] = true;
            $scope.UpdateLogs[index].EditedTitle = $scope.UpdateLogs[index].Title;
            $scope.UpdateLogs[index].EditedDescription = $scope.UpdateLogs[index].Description;
        } else {
            $scope.IsEditUpdateLog[index] = false;
        }
    }

    $scope.resetEditUpdateLog = function (index) {
        $scope.IsEditUpdateLog[index] = false;
        $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
        $scope.updateLogForm.$setPristine();
        $scope.updateLogForm.$setUntouched();
    }
    // Edit updateLog
    $scope.editUpdateLog = function (form) {
        // Put update project
        var promiseEditUpdateLog = ProjectService.editUpdateLogs($scope.UpdateLogs);

        promiseEditUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    // re-set original update log
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                    // reset show update log
                    for (var i = 0; i < $scope.UpdateLogs.length; i++) {
                        if ($scope.IsEditUpdateLog[i] === true) {
                            $scope.showEditUpdateLog(i);
                        }
                    };
                    form.$setPristine();
                    form.$setUntouched();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Edit single updateLog
    $scope.editSingleUpdateLog = function (index) {
        // Put update project
        var promiseEditUpdateLog = ProjectService.editSingleUpdateLogs($scope.UpdateLogs[index]);

        promiseEditUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    //$scope.UpdateLogs[index] = result.data.Data;
                    // re-set original update log
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                    // reset show update log
                    $scope.showEditUpdateLog(index);
                    var flagUpdateLog = 0;
                    for (var i = 0; i < $scope.IsEditUpdateLog.length; i++) {
                        if ($scope.IsEditUpdateLog[i] === true) {
                            flagUpdateLog = 1;
                        }
                    }
                    if (flagUpdateLog === 0) {
                        $scope.updateLogForm.$setPristine();
                        $scope.updateLogForm.$setUntouched();
                    }
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Create a new updateLog
    $scope.createUpdateLog = function () {
        var promiseCreateUpdateLog = ProjectService.createUpdateLog($scope.Project.ProjectID, $scope.NewUpdateLog);

        promiseCreateUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo cập nhật thành công!');
                    $('#updateModal').modal('hide');
                    // reinitial newUpdateLog
                    $scope.NewUpdateLog = {};
                    $scope.CreateUpdateLogForm.$setPristine();
                    $scope.CreateUpdateLogForm.$setUntouched();

                    result.data.Data.CreatedDate = new Date($filter('date')(result.data.Data.CreatedDate, "yyyy-MM-dd"));
                    var dd = result.data.Data.CreatedDate.getDate();
                    var mm = result.data.Data.CreatedDate.getMonth() + 1; //January is 0!
                    var yyyy = result.data.Data.CreatedDate.getFullYear();

                    if (dd < 10) {
                        dd = '0' + dd;
                    }

                    if (mm < 10) {
                        mm = '0' + mm;
                    }
                    result.data.Data.CreatedDate = mm + '/' + dd + '/' + yyyy;
                    $scope.UpdateLogs.unshift(result.data.Data);
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Delete a updateLog
    $scope.deleteUpdateLog = function (index) {
        SweetAlert.swal({
            title: "Xóa cập nhật!",
            text: "Bạn có chắc muốn xóa cập nhật này?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có!",
            cancelButtonText: "Không!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
           function (isConfirm) {
               if (isConfirm) {
                   var promiseDeleteUpdateLog = ProjectService.deleteUpdateLog($scope.UpdateLogs[index].UpdateLogID);

                   promiseDeleteUpdateLog.then(
                       function (result) {
                           if (result.data.Status === "success") {
                               toastr.success('Xóa thành công!');
                               $scope.UpdateLogs.splice(index, 1);
                               $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                           } else {
                               CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                               $scope.Error = result.data.Message;
                               toastr.error($scope.Error, 'Lỗi!');
                           }
                       },
                       function (error) {
                           $scope.Error = error.data.Message;
                           toastr.error($scope.Error, 'Lỗi!');
                       });
               } else {
               }
           });
    };

    // Get timeline records
    $scope.getTimeline = function () {
        if (!$scope.FirstLoadTimeLine) {
            var promiseGetTimeline = ProjectService.getTimeline($scope.Project.ProjectID);

            promiseGetTimeline.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Timeline = result.data.Data;
                        // convert datetime to date
                        for (var i = 0; i < $scope.Timeline.length; i++) {
                            $scope.Timeline[i].DueDate = new Date($filter('date')($scope.Timeline[i].DueDate, "yyyy-MM-dd"));
                        };
                        // Copy original update log
                        $scope.originalTimeline = angular.copy($scope.Timeline);
                        $scope.FirstLoadTimeLine = true;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };
    // Update timeline
    $scope.updateTimeline = function (index) {
        $scope.editTimeline[index] = false;

        var promiseUpdateTimeline = ProjectService.updateTimeline($scope.Timeline[index], $scope.file);

        promiseUpdateTimeline.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    $scope.Timeline.sort(function (a, b) {
                        return new Date(a.DueDate) - new Date(b.DueDate);
                    });
                    // re-set original timeline
                    $scope.originalTimeline = angular.copy($scope.Timeline);
                    $scope.file = null;
                    $("#title" + index).prop('disabled', true);
                    $("#date" + index).prop('disabled', true);
                    $("#" + index).prop('disabled', true);
                    $("#desc" + index).prop('disabled', true);
                    //form.$setPristine();
                    //form.$setUntouched();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Create new timeline point
    $scope.createTimeline = function () {
        if ($scope.newTimeline.DueDate == null) {
            $scope.newTimeline.DueDate = new Date($.now($scope.Timeline));
        }

        var promiseCreateTimeline = ProjectService.createTimeline($scope.Project.ProjectID, $scope.newTimeline, $scope.file);

        promiseCreateTimeline.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo mốc lịch trình thành công!');
                    $('#addTimeline').modal('hide');
                    // reinitial new timeline point
                    $scope.newTimeline = {};
                    // Add new timeline to list
                    result.data.Data.DueDate = new Date($filter('date')(result.data.Data.DueDate, "yyyy-MM-dd"));
                    $scope.Timeline.push(result.data.Data);
                    $scope.Timeline.sort(function (a, b) {
                        return new Date(a.DueDate) - new Date(b.DueDate);
                    });
                    // Reset original timeline
                    $scope.originalTimeline = angular.copy($scope.Timeline);
                    // reset file
                    $scope.file = null;
                    // reset field input img
                    var resetImg = $("#newTimelineImg");
                    resetImg.replaceWith(resetImg = resetImg.clone(true));
                    $scope.isCreateTimeline = false;
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Delete timeline
    $scope.deleteTimeline = function (index) {
        var promiseDeleteReward = ProjectService.deleteTimeline($scope.Timeline[index].TimelineID);

        promiseDeleteReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Xóa thành công!');
                    $scope.Timeline.splice(index, 1);
                    // re-set original timeline
                    $scope.originalTimeline = angular.copy($scope.Timeline);
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };

    // Get question records
    $scope.getQuestion = function () {
        if (!$scope.FirstLoadQA) {
            var promiseGetQuestion = ProjectService.getQuestion($scope.Project.ProjectID);

            promiseGetQuestion.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Question = result.data.Data;
                        $scope.originalQuestion = angular.copy($scope.Question);
                        $scope.FirstLoadQA = true;
                    } else {
                        CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        }
    };

    // Function show edit update log form.
    $scope.showEditQA = function (index) {
        if ($scope.IsEditQA[index] == null || $scope.IsEditQA[index] === false) {
            $scope.IsEditQA[index] = true;
            $scope.Question[index].EditedQuestionContent = $scope.Question[index].QuestionContent;
            $scope.Question[index].EditedAnswer = $scope.Question[index].Answer;
        } else {
            $scope.IsEditQA[index] = false;
        }
    }
    $scope.resetEditQA = function (index) {
        $scope.IsEditQA[index] = false;
        $scope.Question = angular.copy($scope.originalQuestion);
        $scope.questionForm.$setPristine();
        $scope.questionForm.$setUntouched();
    }
    // Edit question
    $scope.updateQuestion = function (form) {
        var promiseUpdateQuestion = ProjectService.editQuestion($scope.Question);

        promiseUpdateQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    // re-set original QA
                    $scope.originalQuestion = angular.copy($scope.Question);
                    // reset show QA
                    for (var i = 0; i < $scope.Question.length; i++) {
                        if ($scope.IsEditQA[i] === true) {
                            $scope.showEditQA(i);
                        }
                    };
                    form.$setPristine();
                    form.$setUntouched();
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Edit single QA
    $scope.editSingleQA = function (index) {
        // Put update project
        var promiseEditUpdateLog = ProjectService.editSingleQuestion($scope.Question[index]);

        promiseEditUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!');
                    //$scope.UpdateLogs[index] = result.data.Data;
                    // re-set original QA
                    $scope.originalQuestion = angular.copy($scope.Question);
                    // reset show update log
                    $scope.showEditQA(index);
                    var flagQA = 0;
                    for (var i = 0; i < $scope.IsEditQA.length; i++) {
                        if ($scope.IsEditQA[i] === true) {
                            flagQA = 1;
                        }
                    }
                    if (flagQA === 0) {
                        $scope.questionForm.$setPristine();
                        $scope.questionForm.$setUntouched();
                    }
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Create a new question
    $scope.createQuestion = function () {
        var promiseCreateQuestion = ProjectService.createQuestion($scope.Project.ProjectID, $scope.newQuestion);

        promiseCreateQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo hỏi đáp thành công!');
                    $('#addQuestion').modal('hide');
                    // reinitial newReward
                    $scope.newQuestion = {};
                    $scope.Question.push(result.data.Data);
                    $scope.originalQuestion = angular.copy($scope.Question);
                } else {
                    CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
    // Delete a question
    $scope.deleteQuestion = function (index) {
        SweetAlert.swal({
            title: "Xóa hỏi đáp.",
            text: "Bạn có chắc muốn xóa hỏi đáp này?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
           function (isConfirm) {
               if (isConfirm) {
                   var promiseDeleteQuestion = ProjectService.deleteQuestion($scope.Question[index].QuestionID);

                   promiseDeleteQuestion.then(
                       function (result) {
                           if (result.data.Status === "success") {
                               toastr.success('Xóa thành công!');
                               $scope.Question.splice(index, 1);
                               $scope.originalQuestion = angular.copy($scope.Question);
                           } else {
                               CommmonService.checkError(result.data.Type, $rootScope.BaseUrl);
                               $scope.Error = result.data.Message;
                               toastr.error($scope.Error, 'Lỗi!');
                           }
                       },
                       function (error) {
                           $scope.Error = error.data.Message;
                           toastr.error($scope.Error, 'Lỗi!');
                       });
               } else {
               }
           });
    };

    // check dirty form
    $scope.dirtyForm = function () {
        if ($scope.fileIsBig == true) {
            $scope.BasicForm.$invalid = true;
        }
        var form;
        if (!angular.equals($scope.originalProjectBasic, $scope.Project) || !angular.equals($scope.originalSelectedCate, $scope.selectedOption)) {
            form = $scope.BasicForm;
            if ($scope.BasicForm.$invalid || (($scope.Project.SubDescription.length > 135 || $scope.Project.SubDescription.length < 30) && $scope.PrjApprove === true)) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditProjectBasic($scope.BasicForm);
            }
            //e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            form = $scope.rewardForm;
            if ($scope.rewardForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditReward($scope.rewardForm);
            }
            //e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            form = $scope.updateLogForm;
            if ($scope.updateLogForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditUpdateLog($scope.updateLogForm);
            }
            //e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            form = $scope.storyForm;
            if ($scope.storyForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditStory($scope.storyForm);
            }
            //e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalQuestion, $scope.Question)) {
            form = $scope.questionForm;
            if ($scope.questionForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditQuestion($scope.questionForm);
            }
            //e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalTimeline, $scope.Timeline)) {
            form = $scope.timelineForm;
            if ($scope.timelineForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditTimeline($scope.timelineForm);
            }
            //e.stopImmediatePropagation();
        }
    };
    // Prevent switch tab if tab's invalid
    $('#tablist a').click(function (e) {
        if ($scope.fileIsBig === true) {
            $scope.BasicForm.$invalid = true;
        }
        var form;
        if (!angular.equals($scope.originalProjectBasic, $scope.Project) || !angular.equals($scope.originalSelectedCate, $scope.selectedOption)) {
            form = $scope.BasicForm;
            if ($scope.BasicForm.$invalid || (($scope.Project.SubDescription.length > 135 || $scope.Project.SubDescription.length < 30) && $scope.PrjApprove === true)) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditProjectBasic($scope.BasicForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            form = $scope.rewardForm;
            if ($scope.rewardForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditReward($scope.rewardForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            form = $scope.updateLogForm;
            if ($scope.updateLogForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditUpdateLog($scope.updateLogForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            form = $scope.storyForm;
            if ($scope.storyForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditStory($scope.storyForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalQuestion, $scope.Question)) {
            form = $scope.questionForm;
            if ($scope.questionForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditQuestion($scope.questionForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalTimeline, $scope.Timeline)) {
            form = $scope.timelineForm;
            if ($scope.timelineForm.$invalid) {
                $scope.checkForm(form);
            } else {
                $scope.checkEditTimeline($scope.timelineForm);
            }
            e.stopImmediatePropagation();
        }

        // Get the tab want to move to
        $scope.thisTab = $(this);
    });

    // If form is dirty
    $scope.checkForm = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa chỉnh sửa sai dữ liệu.",
            text: "Bạn có muốn chỉnh sửa lại không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không, Khôi phục dữ liệu cũ",
            closeOnConfirm: true,
            closeOnCancel: true
        },
           function (isConfirm) {
               if (isConfirm) {
               } else {
                   //SweetAlert.swal("Hủy Bỏ", "Project's basic is safe :)", "error");
                   $scope.Project = angular.copy($scope.originalProjectBasic);
                   $scope.selectedCate();
                   $scope.selectedOption = angular.copy($scope.originalSelectedCate);
                   $(".projecImg-preview").attr("src", $scope.Project.ImageUrl);
                   //reset file field
                   var resetImg = $("#imgSelected");
                   resetImg.replaceWith(resetImg = resetImg.clone(true));
                   $scope.file = null;

                   $scope.RewardPKgs = angular.copy($scope.originalReward);
                   $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
                   $scope.ProjectStory = angular.copy($scope.originalStory);
                   $scope.Question = angular.copy($scope.originalQuestion);
                   $scope.Timeline = angular.copy($scope.originalTimeline);

                   $scope.resetShowUpdateLogAndQA();
                   form.$setPristine();
                   form.$setUntouched();
                   $scope.fileIsBig = false;
                   // Switch tab
                   $scope.changeTab($scope.thisTab.context.hash);
               }
           });
    };

    $scope.resetShowUpdateLogAndQA = function () {
        // reset show update log
        if ($scope.UpdateLogs != null) {
            for (var i = 0; i < $scope.UpdateLogs.length; i++) {
                if ($scope.IsEditUpdateLog[i] === true) {
                    $scope.showEditUpdateLog(i);
                }
            };
        }

        // reset show QA
        if ($scope.Question != null) {
            for (var j = 0; j < $scope.Question.length; j++) {
                if ($scope.IsEditQA[j] === true) {
                    $scope.showEditQA(j);
                }
            };
        }
    }

    // If tab basic is dirty
    $scope.checkEditProjectBasic = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi trang thông tin cơ bản.",
            text: "Thông tin cơ bản của dự án sẽ bị chỉnh sửa. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's basic has been edited.", "success");
                    $scope.editProjectBasic(form);
                } else {
                    SweetAlert.swal("Cancelled", "Project's basic is safe :)", "error");
                    $scope.Project = angular.copy($scope.originalProjectBasic);
                    $scope.selectedCate();
                    $scope.selectedOption = angular.copy($scope.originalSelectedCate);
                    form.$setPristine();
                    form.$setUntouched();
                    $(".projecImg-preview").attr("src", $scope.Project.ImageUrl);
                    //reset file field
                    var resetImg = $("#imgSelected");
                    resetImg.replaceWith(resetImg = resetImg.clone(true));
                    $scope.file = null;
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab reward is dirty
    $scope.checkEditReward = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi thông tin gói quà.",
            text: "Thông tin gói quà của dự án sẽ bị chỉnh sửa. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Reward has been edited.", "success");
                    $scope.updateReward(form);
                } else {
                    SweetAlert.swal("Cancelled", "Reward is safe :)", "error");
                    $scope.RewardPKgs = angular.copy($scope.originalReward);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab update log is dirty
    $scope.checkEditUpdateLog = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi cập nhật dự án.",
            text: "Nội dung cập nhật dự án sẽ bị thay đổi. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's update log has been edited.", "success");
                    $scope.editUpdateLog(form);
                } else {
                    SweetAlert.swal("Cancelled", "Project's update log is safe :)", "error");
                    $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab story is dirty
    $scope.checkEditStory = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi mô tả chi tiết dự án.",
            text: "Nội dung chi tiết dự án sẽ bị chỉnh sửa. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's story has been edited.", "success");
                    $scope.editProjectStory(form);
                } else {
                    SweetAlert.swal("Cancelled", "Project's story is safe :)", "error");
                    $scope.ProjectStory = angular.copy($scope.originalStory);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab story is dirty
    $scope.checkEditQuestion = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi mục hỏi đáp.",
            text: "Nội dung hỏi đáp sẽ bị thay đổi. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's Q&A has been edited.", "success");
                    $scope.updateQuestion(form);
                } else {
                    SweetAlert.swal("Cancelled", "Project's Q&A is safe :)", "error");
                    $scope.Question = angular.copy($scope.originalQuestion);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab timeline is dirty
    $scope.checkEditTimeline = function (form) {
        SweetAlert.swal({
            title: "Bạn vừa thay đổi mốc lịch trình.",
            text: "Lịch trình dự án sẽ bị chỉnh sửa. Bạn có đồng ý không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Không",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's timeline has been edited.", "success");
                    for (var i = 0; i < $scope.Timeline.length; i++) {
                        if ($scope.Timeline[i] !== $scope.originalTimeline[i]) {
                            $scope.updateTimeline(i);
                            break;
                        };
                    }
                } else {
                    SweetAlert.swal("Cancelled", "Project's timeline is safe :)", "error");
                    $scope.Timeline = angular.copy($scope.originalTimeline);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.resetShowUpdateLogAndQA();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // Change tab
    $scope.changeTab = function (toTab) {
        if (toTab === "#reward") {
            $scope.getReward();
        }

        if (toTab === "#story") {
            $scope.getStory();
        }

        if (toTab === "#update") {
            $scope.getUpdateLog();
        }

        if (toTab === "#quesion-answer") {
            $scope.getQuestion();
        }

        if (toTab === "#timelineTab") {
            $scope.getTimeline();
        }

        $scope.thisTab.tab('show');
    };

    // Discard project basic
    $scope.resetProjectBasic = function (form) {
        if (form) {
            $scope.Project = angular.copy($scope.originalProjectBasic);
            $scope.selectedCate();
            $scope.selectedOption = angular.copy($scope.originalSelectedCate);
            form.$setPristine();
            form.$setUntouched();
            $(".projecImg-preview").attr("src", $scope.Project.ImageUrl);
            //reset file field
            var resetImg = $("#imgSelected");
            resetImg.replaceWith(resetImg = resetImg.clone(true));
            $scope.file = null;
            $scope.fileIsBig = false;
        }
    };
    // Discard reward
    $scope.resetReward = function (form) {
        if (form) {
            $scope.RewardPKgs = angular.copy($scope.originalReward);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard story
    $scope.resetStory = function (form) {
        if (form) {
            $scope.ProjectStory = angular.copy($scope.originalStory);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard update log
    $scope.resetUpdateLog = function (form) {
        if (form) {
            $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard question
    $scope.resetQuestion = function (form) {
        if (form) {
            $scope.Question = angular.copy($scope.originalQuestion);
            form.$setPristine();
            form.$setUntouched();
        }
    };

    $scope.turnEditTimeline = function (index) {
        $scope.editTimeline[index] = true;
        $("#title" + index).prop('disabled', false);
        $("#date" + index).prop('disabled', false);
        $("#" + index).prop('disabled', false);
        $("#desc" + index).prop('disabled', false);
    };

    $scope.onNewPoint = function () {
        $scope.isCreateTimeline = true;
    };

    $scope.offNewPoint = function () {
        $scope.isCreateTimeline = false;
    };

    // submit project
    $scope.submit = function () {
        $scope.dirtyForm();
        // Put update project
        var promisePut = ProjectService.submitProject($scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Gửi dự án thành công!');
                    $scope.Project = result.data.Data;
                    $scope.errorListFlag = false;
                    $scope.AllEditable = false;
                    $scope.originalProjectBasic = angular.copy($scope.Project);

                } else {
                    $scope.errorListFlag = true;
                    $scope.errorList = result.data.Data;
                    //toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                //toastr.error($scope.Error, 'Lỗi!');
            });
    }

    // preview project
    $scope.preview = function () {
        $scope.dirtyForm();

        $location.path("/project/detail/" + $scope.Project.ProjectCode).replace();
    }
});