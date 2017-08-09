/**
 * Created by huanghx on 2017/6/28.
 */

import config from '../common/config'
class FileService {
    // 下载文件
    downloadTempFile({fileType, fileToken, fileName}) {
        // window.open(`${config.apiHost}/File/DownloadTempFile?fileType=${fileType}&fileToken=${fileToken}&fileName=${fileName}`)
        window.location = `${config.apiHost}/File/DownloadTempFile?fileType=${fileType}&fileToken=${fileToken}&fileName=${fileName}`
    }
}
export default new FileService()