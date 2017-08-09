   
 

 //第一次使用,请按照如下使用

 //建议将NG1下所有的文件 剪切到APP/common/views文件夹中

 /* 我这里是注释，需要进行路由的配置。将以下代码复制到app.js中功能路由的地方。 */
  //COMMON routes
  /***

  if (abp.auth.hasPermission('Pages.SmsMessagelog')) {
            $stateProvider.state('smsMessagelogs', {
                url: '/smsMessagelogs?filterText',
                templateUrl: '~/App/common/views/smsMessagelogs/index.cshtml',
            });
        }


 */