import authUtils from '../../common/utils/authUtils'
import {
    AUTH_SETUSER
} from '../mutations'

const Auth = {
    state: {
        user: authUtils.getUserInfo() || {}
    },

    mutations: {
        [AUTH_SETUSER] (state, {user}) {
            state.user = user
        }
    }
}

export default Auth
