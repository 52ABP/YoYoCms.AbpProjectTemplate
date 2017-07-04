
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:52. All Rights Reserved.
//<生成时间>2017-07-03T17:31:52</生成时间>
namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs
{
	
			//TODO:Angular后端的导航栏目设计

	/*
	//无次级导航属性
	   var smsMessagelog=new MenuItemDefinition(
                SmsMessagelogAppPermissions.SmsMessagelog,
                L("SmsMessagelog"),
				url:"smsMessagelogs",
				icon:"icon-grid",
				 requiredPermissionName: SmsMessagelogAppPermissions.SmsMessagelog
				);

*/
				//有下级菜单
            /*

			   var smsMessagelog=new MenuItemDefinition(
                SmsMessagelogAppPermissions.SmsMessagelog,
                L("SmsMessagelog"),			
				icon:"icon-grid"				
				);

				smsMessagelog.AddItem(
			new MenuItemDefinition(
			SmsMessagelogAppPermissions.SmsMessagelog,
			L("SmsMessagelog"),
			"icon-star",
			url:"smsMessagelogs",
			requiredPermissionName: SmsMessagelogAppPermissions.SmsMessagelog));
	

			
			*/
	
			
	
	
	
	//配置权限模块初始化
 //TODO:★需要到请将以下内容剪切到AbpProjectTemplateApplicationModule
 //   Configuration.Authorization.Providers.Add<SmsMessagelogAppAuthorizationProvider>();


 

//TODO:★请将以下内容剪切到CORE类库的Localization/Source/AbpProjectTemplatezh-cn.xml
//第一次加载建议复制他
// <text name="MustBe_Required" value="不能为空" />


/*
    <!-- 短信日志记录表管理 -->
	    <text name="SmsMessagelog" value="短信日志记录表" />
	    <text name="SmsMessagelogHeaderInfo" value="短信日志记录表管理列表" />
		    <text name="CreateSmsMessagelog" value="新增短信日志记录表" />
    <text name="EditSmsMessagelog" value="编辑短信日志记录表" />
    <text name="DeleteSmsMessagelog" value="删除短信日志记录表" />

	  
		

		    <text name="SmsMessagelogDeleteWarningMessage" value="短信日志记录表名称: {0} 将被删除,是否确定删除它。" />
<!--//用于表格展示的数据信息的标题-->
					<text name="PhoneNumber" value="发送电话" />
				 	<text name="Content" value="发送内容" />
				 	<text name="SmsCode" value="短信代码(验证码)" />
				 	<text name="InvalidTime" value="失效时间" />
				 	<text name="Sucess" value="是否发送成功" />
				 	<text name="Result" value="短信提供商返回内容" />
				 	<text name="IsChecked" value="是否已检测过" />
				 	<text name="IsNotification" value="是否为通知短信" />
				 	<text name="LastModificationTime" value="最后编辑时间" />
				 	<text name="CreationTime" value="创建时间" />
				 
*/


//TODO:★请将以下内容剪切到CORE类库的Localization/Source/AbpProjectTemplate.xml
/*
    <!-- 短信日志记录表english管理 -->
		    <text name="	SmsMessagelogHeaderInfo" value="短信日志记录表List" />
			<!--//用于表格展示的数据信息的标题-->
					<text name="PhoneNumber" value="发送电话" />
				 	<text name="Content" value="发送内容" />
				 	<text name="SmsCode" value="短信代码(验证码)" />
				 	<text name="InvalidTime" value="失效时间" />
				 	<text name="Sucess" value="是否发送成功" />
				 	<text name="Result" value="短信提供商返回内容" />
				 	<text name="IsChecked" value="是否已检测过" />
				 	<text name="IsNotification" value="是否为通知短信" />
				 	<text name="LastModificationTime" value="最后编辑时间" />
				 	<text name="CreationTime" value="创建时间" />
				 
    <text name="SmsMessagelog" value="ManagementSmsMessagelog" />
    <text name="CreateSmsMessagelog" value="CreateSmsMessagelog" />
    <text name="EditSmsMessagelog" value="EditSmsMessagelog" />
    <text name="DeleteSmsMessagelog" value="DeleteSmsMessagelog" />
*/




}