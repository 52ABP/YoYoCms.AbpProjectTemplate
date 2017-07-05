/**
 * Created by huanghx on 2017/6/26.
 */
//
import fileSerivce from '../fileService'
import serviceHelper from '../serviceHelper'
let webLog = serviceHelper.requireService('webLog')

// 导出所有
webLog.exportAll = async function () {
    let ret = await webLog.downloadWebLogs()
    fileSerivce.downloadTempFile(ret)
}
export default webLog