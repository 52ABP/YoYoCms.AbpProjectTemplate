/**
 * Created by huanghuixin on 2017/3/29.
 */
import {
    INDEX_SETACTIVEMENU
} from '../mutations'

const Auth = {
    state: {
        webpathMain: '主页',
        webpathSub: '',
        navMenueActive: '', // 当前菜单的选中项
    },

    mutations: {
        // 设置选中的菜单
        [INDEX_SETACTIVEMENU] (state, {menu}) {
            state.navMenueActive = menu
        },
    }
}

export default Auth
