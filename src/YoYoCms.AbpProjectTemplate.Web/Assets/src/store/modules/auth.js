import {
    INDEX_SETWEBPATH
} from '../mutations'

const Auth = {
    state: {
        user: {}
    },

    mutations: {
        [INDEX_SETWEBPATH] (state, user) {
            state.user = user
        }
    }
}

export default Auth
