/**
 * Created by huanghx on 2017/6/26.
 */
//
import fileService from './fileService'
import apiHelper from './apiHelper'
let userService = abp.services.app.user
userService.exportExcel = async function () {
    let ret = await userService.getUsersToExcel()
    fileService.downloadTempFile(ret)
}

userService.login = async function (params) {
    // {
    //     url: `/Account/Login?returnUrl=/Application`,
    //         methods: 'post',
    //     data: this.fetchParam,
    //     contentType: 'application/x-www-form-urlencoded'
    // }
    params.returnUrlHash = '#!/h'
    let ret = await apiHelper.post('/Account/Login?returnUrl=none', params, {
        contentType: 'application/x-www-form-urlencoded'
    })
    return ret
}
export default userService