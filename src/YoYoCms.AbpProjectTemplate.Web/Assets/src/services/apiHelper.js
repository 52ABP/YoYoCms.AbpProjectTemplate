/**
 * Created by huanghx on 2017/6/23.
 */

import authUtils from '../common/utils/authUtils'
class ApiHelper {
    get(url, param) {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: url,
                data: param,
                headers: {
                    Authorization: `Bearer ${authUtils.getToken()}`
                },
                success(ret) {
                    resolve(ret)
                },
                error (err) {
                    reject(err)
                }
            })
        })
    }
}

export default new ApiHelper()