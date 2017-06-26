import * as types from './mutations'

const actions = {
    //  设置选中的导航
    setIndexMenuActive (store, {menu}) {
        if (abp.nav && menu) {
            menu.map((item) => {
                item.displayName = searchcDisplayName(item.name, abp.nav.menus.MainMenu)
            })
        }
        store.commit(types.INDEX_SETACTIVEMENU, {menu})
    },
}

// 从abp.nav中 获取菜单显示的名字
function searchcDisplayName(name, nav) {
    if (nav.name === name) return nav.displayName
    if (!nav.items || nav.items.length < 1) return null
    let ret
    for (let k in nav.items) {
        let item = nav.items[k]
        ret = searchcDisplayName(name, item)
        if (ret) break
    }
    return ret
}
export default actions
