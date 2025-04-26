"use client";

import { useRef, useEffect, useState } from "react";
import { EditorView, basicSetup } from "codemirror";
import { javascript } from "@codemirror/lang-javascript";
import { oneDark } from "@codemirror/theme-one-dark";
import { EditorState } from "@codemirror/state";
import { Copy, Check, Play } from "lucide-react";

export default function CodeEditor() {
  const editorRef = useRef<HTMLDivElement>(null);
  const [editorView, setEditorView] = useState<EditorView | null>(null);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    if (editorRef.current && !editorView) {
      const state = EditorState.create({
        doc: `// Start typing your code...
function hello() {
  console.log("Hello World!");
}`,
        extensions: [
          basicSetup,
          javascript(),
          oneDark,
          EditorView.theme({
            "&": {
              height: "100%",
              backgroundColor: "transparent",
              color: "var(--tw-prose-body)",
              fontFamily: "monospace",
              fontSize: "14px",
            },
            ".cm-content": {
              padding: "1rem",
            },
            ".cm-gutters": {
              backgroundColor: "transparent",
              border: "none",
            },
          }),
        ],
      });

      const view = new EditorView({
        state,
        parent: editorRef.current,
      });

      setEditorView(view);
    }
  }, [editorRef, editorView]);

  const handleCopy = () => {
    if (editorView) {
      navigator.clipboard.writeText(editorView.state.doc.toString());
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    }
  };

  return (
    <div className="relative w-full h-full border bg-muted/40 shadow-md overflow-hidden flex flex-col">
      {/* Darker Bar Above the Compiler */}
      <div className="w-full h-[35px] bg-gray-900 flex items-center justify-center gap-5 ">
      <button className="flex items-center justify-center pl-[8px] text-white font-medium rounded-md border border-white h-7 w-10">
      <Play className="h-4 w-5 mr-2 " />
        </button>

      <button
          onClick={handleCopy}
          className="rounded-md p-2 bg-background/70 text-white hover:bg-background border text-foreground text-xs transition"
        >
          {copied ? <Check className="w-4 h-3 text-green-500" /> : <Copy className="w-4 h-3" />}
        </button>
    </div>

      {/* Container for the Editor and Button */}
      <div className="flex flex-grow justify-start items-start">
        <div ref={editorRef} className="w-full h-full" />
      </div>
    </div>
  );
  
}
