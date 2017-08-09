/**
 * Created by huanghx on 2017/6/21.
 */
export default {
    // 加载js
    loadJs(url) {
        let script = document.createElement('script')
        script.src = url
        document.body.appendChild(script)
        return new Promise((resolve, reject) => {
            script.onload = function () {
                resolve(true)
            }

            script.onerror = function (e) {
                reject(e)
            }
        })
    }
}
