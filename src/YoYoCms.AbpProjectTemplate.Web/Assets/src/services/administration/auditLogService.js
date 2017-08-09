/**
 * Created by huanghx on 2017/6/28.
 */
import fileService from '../fileService'
import serviceHelper from '../serviceHelper'
let auditLog = serviceHelper.requireService('auditLog')
auditLog.exportExcel = async function (params) {
    let ret = await auditLog.getAuditLogsToExcel(params)
    fileService.downloadTempFile(ret)
}
export default auditLog