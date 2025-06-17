VAR grade = ""

-> start

=== start ===
바르도 세계에 오신걸 환영합니다.

당신은 몇학년?
    *[1학년]
        ~ grade = 1
        새내기시군요.
        -> start2
    *[2학년]
        ~ grade = 2
        2학년이시군요.
        -> start2
    *[3학년]
        ~ grade = 3
        3학년이시군요.
        -> start2
    *[4학년]
        ~ grade = 4 
        4학년이시군요.
        -> start2
        
=== start2 ===
당신은 몇학기?
    *[1학기]
        {grade}학년 1학기시군요.
    *[2학기]
        {grade}학년 2학기시군요.
- -> END