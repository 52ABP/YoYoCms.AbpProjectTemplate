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
        navMenueActive: [], // 当前菜单的选中项
        navShow: []
    },

    mutations: {
        // 设置选中的菜单
        [INDEX_SETACTIVEMENU] (state, {menu}) {
            state.navMenueActive = menu

            if (menu && menu.length > 1) {
                let navShow = menu.filter((item) => {
                    return !!item.displayName
                })
                state.navShow = navShow || []
            }
        },
    },
}

export default Auth
