/**
 * Created by huanghx on 2017/6/26.
 */
//

import serviceHelper from '../serviceHelper'
// import apiHelper from './apiHelper'
let profile = serviceHelper.requireService('profile')

// // 上传头像
// profile.uploadPortrait = async function ({imgData, fileName}) {
//     return await apiHelper.post('/Profile/UploadPortrait', {imgData, fileName})
// }
export default profile