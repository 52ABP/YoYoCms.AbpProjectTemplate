/**
 * Created by huanghx on 2017/6/26.
 */
//
import fileService from './fileService'

let userService = abp.services.app.user
userService.exportExcel = async function () {
    let ret = await userService.getUsersToExcel()
    fileService.downloadTempFile(ret)
}
export default userService