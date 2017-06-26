import * as types from './mutations'

const actions = {
    //  设置选中的导航
    setIndexMenuActive (store, {menu}) {
        store.commit(types.INDEX_SETACTIVEMENU, {menu})
    },
}

export default actions
