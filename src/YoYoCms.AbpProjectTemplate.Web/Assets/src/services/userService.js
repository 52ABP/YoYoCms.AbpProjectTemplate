/**
 * Created by huanghx on 2017/6/26.
 */
//
import fileService from './fileService'
import apiHelper from './apiHelper'
import serviceHelper from './serviceHelper'
let userService = serviceHelper.requireService('user')
userService.exportExcel = async function () {
    let ret = await userService.getUsersToExcel()
    fileService.downloadTempFile(ret)
}

userService.login = async function (params) {
    params.returnUrlHash = '#!/h'
    let ret = await apiHelper.post('/Account/Login?returnUrl=none', params, {
        contentType: 'application/x-www-form-urlencoded'
    })

    return ret
}

// 重置密码
userService.resetPwd = function (params) {
    return apiHelper.post('/Account/ResetPassword', params, {
        contentType: 'application/x-www-form-urlencoded'
    })
}

// 注册
userService.register = function (data) {
    // IsExternalLogin=False&Name=asda&Surname=asdasd&EmailAddress=asd%40asd.asd&UserName=123asd&Password=asdasd123&PasswordRepeat=asdasd123
    // abp.ajax()
    return abp.ajax({
        url: '/Account/Register?type=ajax',
        data,
        method: 'post',
        contentType: 'application/x-www-form-urlencoded'
    })
}

// 发送找回密码链接
userService.sendPasswordResetLink = function (data) {
    return abp.ajax({
        url: '/Account/SendPasswordResetLink',
        data,
        method: 'post',
        contentType: 'application/x-www-form-urlencoded'
    })
}

// 发送激活邮件
userService.sendEmailActivationLink = function (data) {
    return abp.ajax({
        url: '/Account/SendEmailActivationLink',
        data,
        method: 'post',
        contentType: 'application/x-www-form-urlencoded'
    })
}

userService.logout = function () {
    apiHelper.get('/Account/Logout')
}
export default userService