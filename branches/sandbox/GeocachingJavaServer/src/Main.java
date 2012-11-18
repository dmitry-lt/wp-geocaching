import org.mortbay.jetty.Handler;
import org.mortbay.jetty.Request;
import org.mortbay.jetty.Server;
import org.mortbay.jetty.handler.AbstractHandler;

import javax.servlet.ServletException;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Date;
import java.util.Enumeration;

public class Main {

    private static void logRequest(HttpServletRequest request) {
        String fileName = new Date().getTime() + ".req";
        OutputStream out = null;
        try {
            out = new FileOutputStream(fileName);
            InputStream in = request.getInputStream();
            byte[] buffer = new byte[1024];
            int len = in.read(buffer);
            while (len != -1) {
                out.write(buffer, 0, len);
                len = in.read(buffer);
            }
        } catch (IOException e) {
            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
        } finally {
            if (null != out) {
                try {
                    out.close();
                } catch (IOException e) {
                    e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
                }
            }
        }
    }

    private static void logRequestHeaders(HttpServletRequest request) {
//        String fileName = "h_" + new Date().getTime() + ".req";
        OutputStream out = null;
//        try {
//            out = new FileOutputStream(fileName);

        Enumeration<String> headers = request.getHeaderNames();
        while (headers.hasMoreElements()) {
            String h = headers.nextElement();
            String v = request.getHeader(h);
            System.out.println(h + ": " + v);
        }
//        } catch (IOException e) {
//            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
//        } finally {
//            if (null != out) {
//                try {
//                    out.close();
//                } catch (IOException e) {
//                    e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
//                }
//            }
//        }
    }

    private static void logRequestCookies(HttpServletRequest request) {
//        String fileName = "h_" + new Date().getTime() + ".req";
        OutputStream out = null;
//        try {
//            out = new FileOutputStream(fileName);

        Cookie[] cookies = request.getCookies();
        if (null != cookies) {
            for (Cookie c : cookies) {
                System.out.println("Cookie " + c.getName() + ": " + c.getValue());
            }
        }
//        } catch (IOException e) {
//            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
//        } finally {
//            if (null != out) {
//                try {
//                    out.close();
//                } catch (IOException e) {
//                    e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
//                }
//            }
//        }
    }

    public static void main(String[] args) throws Exception {
        Handler handler = new AbstractHandler() {
            public void handle(String target, HttpServletRequest request, HttpServletResponse response, int dispatch)
                    throws IOException, ServletException {
                logRequest(request);
                logRequestHeaders(request);
                logRequestCookies(request);
                response.setContentType("text/html");
                response.setStatus(HttpServletResponse.SC_OK);
                response.getWriter().println("<h1>Hello</h1>");
                ((Request) request).setHandled(true);
            }
        };

        Server server = new Server(8080);
        server.setHandler(handler);
        server.start();
    }
}
