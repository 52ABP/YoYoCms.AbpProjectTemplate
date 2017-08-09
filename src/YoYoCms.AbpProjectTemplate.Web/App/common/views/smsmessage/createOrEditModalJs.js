 
  


// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:43:46. All Rights Reserved.
//<生成时间>2017-07-03T17:43:46</生成时间>

 (function () {
    appModule.controller('common.views.smsMessagelogs.createOrEditModal', [
         '$scope', '$uibModalInstance', 'abp.services.yoyocms.smsMessagelog', 'smsMessagelogId',
        function ($scope, $uibModalInstance, smsMessagelogService, smsMessagelogId) {
            var vm = this;
            vm.saving = false;
            //首先将smsMessagelog数据设置为null
            vm.smsMessagelog = null;

             

            //触发保存方法
            vm.save = function () {
                vm.saving = true;
                smsMessagelogService.createOrUpdateSmsMessagelogAsync({ smsMessagelogEditDto:vm.smsMessagelog }).then(function() {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function() {
                    vm.saving = false;
                });


            };
            //取消关闭页面
            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            //初始化页面
            function init() {
             //   console.log(smsMessagelogId);
                smsMessagelogService.getSmsMessagelogForEditAsync({
                    id: smsMessagelogId
                }).then(function (result) {
              //      console.log(result);
                    //console.log(result.data);
                    vm.smsMessagelog = result.data.smsMessagelog;
					
																																																			   
		   //日期选择器
                    $("#InvalidTime").datetimepicker({
                          minDate: new Date(),
                          autoclose: true,
                          isRTL: false,
                          format: "yyyy-mm-dd hh:ii",
                          pickerPosition: ("bottom-left"),
                          //默认为E文按钮要中文，自己去找语言包
                          todayBtn: true,
                          language: "zh-CN",
                          startDate: new Date(),
                          todayHighlight: true
                          });		 
		  																																											 
				 

                });
            }
            //执行初始化方法
            init();
        }
    ]);
})();