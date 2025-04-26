import React from 'react'
import CodeEditor from '../Components/CodeEditor'



const MainPage = () => {
  return (
    <div className="grid grid-cols-3">
      <div className="bg-red-500 col-span-1 h-screen">

      </div>
      <div className='col-start-2 col-span-2 h-screen'>
        <CodeEditor/>
      </div>
    </div>
  )
}

export default MainPage