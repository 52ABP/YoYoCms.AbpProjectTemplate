/**
 * Created by huanghx on 2017/6/22.
 */

import {Notification} from 'element-ui'

// toastr的实现
class ToastrImp {
    constructor() {
        this.options = {}
    }

    success(message, title) {
        Notification['success']({message, title})
    }

    info(message, title) {
        Notification['info']({message, title})
    }

    warn(message, title) {
        Notification['warn']({message, title})
    }

    error(message, title) {
        Notification['error']({message, title})
    }
}

let toastr = new ToastrImp()
window.toastr = toastr
export default toastr